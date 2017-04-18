using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyTimeline.Services
{
    class EventService
    {
        string[] _placeholderImages = {
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

        public ObservableCollection<Event> GetEvents()
        {
            ObservableCollection<Event> events;
            events = new ObservableCollection<Event>();

            events.Add(new Event { Name = "Alone at Home", Images = GenerateImagesForEvent()});
            events.Add(new Event { Name = "Development of cool app", Images = GenerateImagesForEvent() });
            events.Add(new Event { Name = "Call regarding future", Images = GenerateImagesForEvent() });
            events.Add(new Event { Name = "Drinking a lot of beer", Images = GenerateImagesForEvent() });

            return events;
        }

        private ObservableCollection<EventImage> GenerateImagesForEvent()
        {
            ObservableCollection<EventImage> images = new ObservableCollection<EventImage>();

            int maxNumberPictures = 30;
            Random nrGenerator = new Random(DateTime.Now.Millisecond);
            int numberPictures = nrGenerator.Next() % maxNumberPictures;

            for (int i = 0; i < numberPictures; i++)
            {
                EventImage newEventImage = new EventImage();
                newEventImage.ShortAnnotation = "Default Short Annotation";
                newEventImage.DateTaken = DateTime.Now;
                int nrImages = _placeholderImages.Length;
                nrGenerator = new Random(DateTime.Now.Millisecond);
                newEventImage.Id = nrGenerator.Next();

                int imageIndex = nrGenerator.Next() % nrImages;
                newEventImage.URI = _placeholderImages[imageIndex];

                images.Add(newEventImage);
            }

            return images;
        }
    }
}
