using System;
using System.Diagnostics;

namespace PartyTimeline
{
	public class EventImage
	{
		public long ID { get; set; }
		public DateTime DateTaken { get; set; }
		public string ShortAnnotation { get; set; }
		public string LongAnnotation { get; set; }
		public string URI { get; set; }

		string[] placeholderImages = {
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

		public EventImage()
		{
			InitializeDummyValues();
		}

		void InitializeDummyValues()
		{
			ShortAnnotation = "DefaultShortAnnotation";
			DateTaken = DateTime.Now;
			int nrImages = placeholderImages.Length;
			var nrGenerator = new Random(DateTime.Now.Millisecond);
			ID = nrGenerator.Next();
			int imageIndex = nrGenerator.Next() % nrImages;
			URI = placeholderImages[imageIndex];

			Debug.WriteLine("Initialized Image with index {0} and URI {1}", imageIndex, URI);
		}
	}
}
