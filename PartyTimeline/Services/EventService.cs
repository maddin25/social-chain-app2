using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.Annotations;


// TODO: implement the sqlite interface with mono.data.sqlite, see https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/
namespace PartyTimeline.Services
{
	public class EventService : INotifyPropertyChanged
	{
		private static EventService _instance;
		public List<Event> EventList { get; private set; }

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

		public event PropertyChangedEventHandler PropertyChanged;

		private EventService()
		{
			EventList = new List<Event>();
			QueryLocalEventList();
		}

		[NotifyPropertyChangedInvocator]
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private static ObservableCollection<EventImage> GenerateImagesForEvent()
		{
			ObservableCollection<EventImage> images = new ObservableCollection<EventImage>();

			int maxNumberPictures = 30;
			Random nrGenerator = new Random(DateTime.Now.Millisecond);
			int numberPictures = nrGenerator.Next() % maxNumberPictures;

			for (int i = 0; i < numberPictures; i++)
			{
				EventImage newEventImage = new EventImage(DateTime.Now);
				newEventImage.Caption = "Default Short Annotation";
				int nrImages = _placeholderImages.Length;
				nrGenerator = new Random(DateTime.Now.Millisecond);
				newEventImage.Id = nrGenerator.Next();

				int imageIndex = nrGenerator.Next() % nrImages;
				newEventImage.URI = _placeholderImages[imageIndex];

				images.Add(newEventImage);
			}

			return images;
		}

		public void QueryLocalEventList()
		{
			foreach (Event eventReference in DependencyService.Get<EventListInterface>().ReadLocalEvents())
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
				}
				else
				{
					EventList.Add(eventReference);
				}
			}
			SortEventList();
		}

		public void QueryLocalEventImageList(Event eventReference)
		{
			foreach (EventImage image in DependencyService.Get<EventListInterface>().ReadLocalEventImages(eventReference))
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
			EventList.Find((Event obj) => obj.Equals(eventReference))?.Images.Add(image);
			DependencyService.Get<EventListInterface>().WriteLocalEventImage(image, eventReference);
			DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(image);
			NotifyPropertyChanged(eventReference.Name);
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
			EventList.Sort();
			NotifyPropertyChanged(nameof(EventList));
		}

		private void SortEventImageList(Event eventReference)
		{
			eventReference.Images.OrderByDescending((image) => image.DateCreated.ToFileTimeUtc());
		}
	}
}
