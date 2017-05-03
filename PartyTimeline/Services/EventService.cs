using System.IO;
using System.Threading.Tasks;

using System.Diagnostics;

using Acr.UserDialogs;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class EventService
	{
		private static EventService _instance;
		private LocalDatabaseAccess localDb;
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
		}

		public async Task QueryLocalEventListAsync()
		{
			foreach (Event eventReference in await Task.Run(() => localDb.ReadEvents()))
			{
				int index = EventList.IndexOf(eventReference);
				if (index >= 0)
				{
					if (EventList[index].ModifiedAfter(eventReference))
					{
						/* The event in the EventList is somehow newer than the event stored in the local database. That
						 * should not be. */
						Debug.WriteLine($"WARNING: event '{eventReference.Name}' in local database is outdated!");
					}
					else
					{
						EventList[index] = eventReference;
					}
				}
				else
				{
					EventList.Add(eventReference);
				}
			}
			SortEventList();
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

		public void AddImageToEvent(EventImage image, Event eventReference)
		{
			// TODO: verify that also the "global" event list is being updated
			int index = EventList.IndexOf(eventReference);
			EventList[index].Images.Add(image);
			localDb.WriteEventImage(image, EventList[index]);
			SortEventImageList(eventReference);
			//DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(image);
		}

		public void AddNewEvent(Event eventReference)
		{
			EventList.Add(eventReference);
			localDb.WriteEvent(eventReference);
			SortEventList();
			//DependencyService.Get<EventListInterface>().PushServerEvent(eventReference);
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

		public void Remove(EventImage image)
		{
			long eventId = localDb.RemoveEventImage(image);
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

		public void Remove(BaseModel item)
		{
			if (item is EventImage)
			{
				Remove((EventImage)item);
			}
			else if (item is Event)
			{
				Remove((Event)item);
			}
			else
			{
				Debug.WriteLine($"Deleting instances of {item.GetType()} not supported");
			}
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
			EventList.SortDescending((arg) => arg.DateCreated.ToFileTimeUtc());
		}

		private void SortEventImageList(Event eventReference)
		{
			eventReference.Images.SortDescending((image) => image.DateCreated.ToFileTimeUtc());
		}
	}
}
