using System.Threading.Tasks;

using System.Diagnostics;

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
						Debug.WriteLine($"WARNING: image with URI '{image.URI}' in local database is outdated!");
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
			eventReference.Images.Add(image);
			localDb.WriteEventImage(image, eventReference);
			//DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(image);
		}

		public void AddNewEvent(Event eventReference)
		{
			EventList.Add(eventReference);
			SortEventList();
			localDb.AddEvent(eventReference);
			//DependencyService.Get<EventListInterface>().PushServerEvent(eventReference);
		}

		private void SortEventList()
		{
			((SortableObservableCollection<Event>) EventList).SortDescending((arg) => arg.DateCreated.ToFileTimeUtc());
		}

		private void SortEventImageList(Event eventReference)
		{
			((SortableObservableCollection<EventImage>) eventReference.Images).SortDescending((image) => image.DateCreated.ToFileTimeUtc());
		}
	}
}
