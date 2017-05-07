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
			SessionInformationProvider.INSTANCE.SessionStateChanged += (sender, e) => EventList.Clear();
		}

		public async Task LoadEventList()
		{
			OnSyncStateChanged(new SyncState { IsSyncing = true, SyncService = SyncServices.EventList });
			Task<List<Event>> fbEvents = Task.Run(fbClient.GetEventHeaders);
			List<long> eventsRequiredUpdate = new List<long>();

			List<Event> localEvents = await localDb.ReadEvents();
			foreach (Event e in localEvents)
			{
				AddEventToEventList(e);
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
					else
					{
						AddEventToEventList(le, false);
					}
				}
				else // this event is new
				{
					localDb.AssociateEventMemberWithEvent(fe, SessionInformationProvider.INSTANCE.CurrentUserEventMember, EventMembershipRoles.ROLES.Contributor);
					eventsRequiredUpdate.Add(fe.Id);
				}
			}
			await Task.WhenAll(eventsRequiredUpdate.Select((long id) => UpdateLocalEvent(id)));
			SortEventList();
            OnSyncStateChanged(new SyncState { IsSyncing = false, SyncService = SyncServices.EventList });
		}

		public async Task UpdateLocalEvent(long eventId)
		{
			Event fe = await fbClient.GetEventDetails(eventId);
			localDb.UpdateEvent(fe);
			AddEventToEventList(fe, true);
		}

		public void AddEventToEventList(Event e, bool updateIfExisting = false)
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

		public async Task QueryLocalEventImageList(Event eventReference)
		{
			foreach (EventImage image in await Task.Run(() => localDb.ReadEventImages(eventReference)))
			{
				int index = eventReference.Images.IndexOf(image);
				if (index >= 0)
				{
					if (eventReference.Images[index].ModifiedAfter(image))
					{
						/* The image in the Event is somehow newer than the image stored in the local database. That
						 * should not be. */
						Debug.WriteLine($"WARNING: image with URI '{image.Path}' in local database is outdated!");
					}
					else
					{
						eventReference.Images[index] = image;
					}
				}
				else
				{
					eventReference.Images.Add(image);
				}
			}
			SortEventImageList(eventReference);
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

		private void OnSyncStateChanged(SyncState state)
		{
			Debug.WriteLine($"SyncStateChanged event triggered with state {state.ToString()}");
			SyncStateChanged?.Invoke(this, state);
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
