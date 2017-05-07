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
		private FacebookClient fbClient;

		public EventHandler SyncStateChanged { get; set; }

		public SyncState CurrentSyncState { get; set; }

		public static readonly TimeSpan LimitEventsInPast = TimeSpan.FromDays(180);
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
			fbClient = new FacebookClient();
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
			CurrentSyncState.EventListSyncing = true;
			OnSyncStateChanged();
			Task<List<Event>> fbEvents = Task.Run(fbClient.GetEventHeaders);
			List<long> eventsRequiredUpdate = new List<long>();

			List<Event> localEvents = await localDb.ReadEvents();
			foreach (Event e in localEvents)
			{
				AddToEventList(e);
			}
			// TODO: remove local events that are outdated?

			foreach (Event fe in await fbEvents)
			{
				if (fe.IsDraft || fe.IsCancelled)
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
					// else: it is already in the list and does not need to be updated
				}
				else // this event is new
				{
					localDb.AssociateEventMemberWithEvent(fe, SessionInformationProvider.INSTANCE.CurrentUserEventMember, EventMembershipRoles.ROLES.Contributor);
					eventsRequiredUpdate.Add(fe.Id);
				}
			}
			await Task.WhenAll(eventsRequiredUpdate.Select((long id) => UpdateLocalEvent(id)));
			SortEventList();
			CurrentSyncState.EventListSyncing = false;
			OnSyncStateChanged();
		}

		public async Task LoadEventImageList(Event e)
		{
			CurrentSyncState.EventDetailsSyncing = true;
			CurrentSyncState.EventIdSyncing = e.Id;
			OnSyncStateChanged();
			// TODO pull this list from the server
			List<EventImage> serverEventImages = new List<EventImage>();
			List<EventImage> localEventImages = await localDb.ReadEventImages(e.Id);
			foreach (EventImage img in localEventImages)
			{
				AddToEventImageList(e, img, false);
			}
			/**
			foreach (EventImage serverImage in serverEventImages)
			{
				if (localEventImages.Contains(serverImage))
				{
					EventImage lImg = localEventImages[localEventImages.IndexOf(serverImage)];
					if (lImg.DateLastModified < serverImage.DateLastModified) // the local event image is outdated
					{
						// TODO: mark this image to be updated
					}
				}
				else
				{
					// TODO: mark this image to be updated
				}
			}
			*/
			// TODO: update all marked images
			SortEventImageList(e);
			CurrentSyncState.EventDetailsSyncing = false;
			OnSyncStateChanged();
		}

		public async Task UpdateLocalEvent(long eventId)
		{
			Event fe = await fbClient.GetEventDetails(eventId);
			localDb.UpdateEvent(fe);
			AddToEventList(fe, true);
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

		public void AddImage(EventImage image)
		{
			Event e = EventList.FirstOrDefault((arg) => arg.Id == image.EventId);
			e?.Images.Add(image);
			localDb.WriteEventImage(image);
			SortEventImageList(e);
			//DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(image);
		}

		public void AddEventMember(EventMember member)
		{
			localDb.WriteEventMember(member);
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
			DeleteFileWithDialog(image.Path);
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
