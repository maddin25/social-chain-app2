using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.Annotations;


// TODO: implement the sqlite interface with mono.data.sqlite, see https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/
namespace PartyTimeline.Services
{
	public class EventService
	{
		private static EventService _instance;
		public SortableObservableCollection<Event> EventList { get; private set; }

		static string[] _placeholderImages = {
			"https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
			"https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
			"https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
			"https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
			"https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
			"https://farm2.staticflickr.com/1227/1116750115_b66dc3830e.jpg",
			"https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg",
			"https://farm1.staticflickr.com/44/117598011_250aa8ffb1.jpg",
			"https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
			"https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg"
		};

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
		}

		public async Task QueryLocalEventListAsync()
		{
			foreach (Event eventReference in await Task.Run(() => DependencyService.Get<EventListInterface>().ReadLocalEvents()))
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
			foreach (EventImage image in await Task.Run(() => DependencyService.Get<EventListInterface>().ReadLocalEventImages(eventReference)))
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
			DependencyService.Get<EventListInterface>().WriteLocalEventImage(image, eventReference);
			DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(image);
		}

		public void AddNewEvent(Event eventReference)
		{
			EventList.Add(eventReference);
			SortEventList();
			DependencyService.Get<EventListInterface>().WriteLocalEvent(eventReference);
			DependencyService.Get<EventListInterface>().PushServerEvent(eventReference);
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
