using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Acr.UserDialogs;

using Xamarin.Forms;

namespace PartyTimeline
{
    public class EventService
    {
        private static EventService _instance;
        private LocalDatabaseAccess localDb;
        private FacebookClient clientFb;
        private RestClientEvents clientEvents;
        private RestClientImages clientImages;

        public EventHandler SyncStateChanged { get; set; }

        public SyncState CurrentSyncState { get; set; }

        public static int LimitEventsInPastDays = 180;
        public static readonly TimeSpan LimitEventsInPast = TimeSpan.FromDays(LimitEventsInPastDays);
        public SortableObservableCollection<Event> EventList { get; private set; }

        public static EventService INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventService();
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        private EventService()
        {
            EventList = new SortableObservableCollection<Event>();
            localDb = new LocalDatabaseAccess();
            clientFb = new FacebookClient();
            clientEvents = new RestClientEvents();
            clientImages = new RestClientImages();
            CurrentSyncState = new SyncState();
            SessionInformationProvider.INSTANCE.SessionStateChanged += (sender, e) =>
            {
                if (e is SessionState)
                {
                    SessionState state = e as SessionState;
                    if (state.IsAuthenticated == false)
                    {
                        EventList.Clear();
                    }
                }
            };
        }

        public async Task LoadEventList()
        {
            Debug.WriteLine("Loading event list ...");
            CurrentSyncState.EventListSyncing = true;
            OnSyncStateChanged();
            Task<List<Event>> fbEvents = Task.Run(clientFb.GetEventHeaders);
            List<long> eventsRequiredUpdate = new List<long>();

            List<Event> localEvents = await localDb.ReadEvents();
            foreach (Event e in localEvents)
            {
                AddToEventList(e);
            }
            // TODO: remove local events that are outdated?

            foreach (Event fe in await fbEvents)
            {
                if (fe.IsDraft || fe.IsCanceled)
                {
                    continue;
                }

                if (localEvents.Contains(fe))
                {
                    Event le = localEvents[localEvents.IndexOf(fe)];
                    if (le.DateLastModified < fe.DateLastModified) // the local event is outdated
                    {
                        eventsRequiredUpdate.Add(fe.Id);
                    }
                    else
                    {
                        // HACK: remove this if a proper push routine is defined
                        await clientEvents.PostAsync(le);
                    }
                    // else: it is already in the list and does not need to be updated
                }
                else // this event is new
                {
                    localDb.AssociateEventMemberWithEvent(fe, SessionInformationProvider.INSTANCE.CurrentUserEventMember, EventMembershipRoles.ROLES.Contributor);
                    eventsRequiredUpdate.Add(fe.Id);
                }
            }
            await Task.WhenAll(eventsRequiredUpdate.Select((long id) => UpdateEvent(id)));
            SortEventList();
            CurrentSyncState.EventListSyncing = false;
            Debug.WriteLine("Finished loading event list");
            OnSyncStateChanged();
        }

        public async Task LoadEventImageList(Event e)
        {
            CurrentSyncState.EventDetailsSyncing = true;
            CurrentSyncState.EventIdSyncing = e.Id;
            OnSyncStateChanged();

            Task<List<EventImage>> serverEventImages = Task.Run(() => clientImages.GetEventImages(e.Id));
            List<EventImage> localEventImages = await localDb.ReadEventImages(e.Id);
            foreach (EventImage img in localEventImages)
            {
                AddToEventImageList(e, img, false);
            }

            foreach (EventImage sImg in await serverEventImages)
            {
                bool existingAndUpToDate = localEventImages.Find((img) => img.Equals(sImg))?.DateLastModified >= sImg.DateLastModified;
                if (!existingAndUpToDate)
                {
                    PersistElementLocal(sImg);
                    AddToEventImageList(e, sImg, true);
                }
            }

            SortEventImageList(e);
            CurrentSyncState.EventDetailsSyncing = false;
            OnSyncStateChanged();
        }

        public async Task UpdateEvent(long eventId)
        {
            Event fe = await clientFb.GetEventDetails(eventId);
            PersistElementLocal(fe);
            await clientEvents.PostAsync(fe);
            AddToEventList(fe, true);
            Debug.WriteLine($"Finished updating event with ID {eventId} (local & server)");
        }

        public void PersistElementLocal(BaseModel element)
        {
            localDb.Persist(element);
            // TODO: update changes on server as well if we implement a generic post interface
        }

        public void AddToEventImageList(Event e, EventImage img, bool updateIfExisting = false)
        {
            int index = e.Images.IndexOf(img);
            if (index >= 0)
            {
                if (updateIfExisting)
                {
                    e.Images.Update(index, img);
                }
            }
            else
            {
                e.Images.Add(img);
            }
        }

        public void AddToEventList(Event e, bool updateIfExisting = false)
        {
            int index = EventList.IndexOf(e);
            if (index >= 0)
            {
                if (updateIfExisting)
                {
                    EventList.Update(index, e);
                }
            }
            else
            {
                EventList.Add(e);
            }
        }

        public async void AddImage(EventImage image)
        {
            Event e = EventList.FirstOrDefault((arg) => arg.Id == image.EventId);
            // HACK: using static user id
            image.EventMemberId = 10206756772397816L;
            Task postTask = clientImages.PostAsync(image);
            e?.Images.Add(image);
            PersistElementLocal(image);
            SortEventImageList(e);
            await postTask;
        }

        public async void SmallImageAvailable(EventImage image)
        {
            PersistElementLocal(image);
            bool success = await clientImages.UploadImage(image, ImageQualities.SMALL);
            Debug.WriteLineIf(!success, $"Failed uploading small version of image '{image.PathSmall}'");
        }

        public void Remove(Event eventReference)
        {
            EventList.Remove(eventReference);
            localDb.RemoveEvent(eventReference);
            SortEventList();
        }

        public async Task Remove(EventImage image)
        {
            long eventId = await localDb.RemoveEventImage(image);
            if (eventId != -1)
            {
                int eventIndex = EventList.IndexOf(new Event { Id = eventId });
                if (eventIndex >= 0)
                {
                    EventList[eventIndex].Images.Remove(image);
                }
                SortEventImageList(EventList[eventIndex]);
            }
            DeleteFileWithDialog(image.PathOriginal);
            DeleteFileWithDialog(image.PathSmall);
        }

        private void OnSyncStateChanged()
        {
            Debug.WriteLine($"SyncStateChanged event triggered with state {CurrentSyncState.ToString()}");
            SyncStateChanged?.Invoke(this, CurrentSyncState);
        }

        private bool DeleteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }
            else
            {
                return DependencyService.Get<SystemInterface>().DeleteFile(path);
            }
        }

        private void DeleteFileWithDialog(string path)
        {
            if (!DeleteFile(path))
            {
                UserDialogs.Instance.Alert(Path.GetFileName(path), "Deleting file failed");
            }
        }

        private void SortEventList()
        {
            EventList.SortDescending((e) => e.StartDateTime.ToFileTimeUtc());
        }

        private void SortEventImageList(Event eventReference)
        {
            eventReference.Images.SortDescending((image) => image.DateTaken.ToFileTimeUtc());
        }
    }
}
