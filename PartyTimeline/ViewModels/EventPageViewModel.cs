using System;
using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.Views;

namespace PartyTimeline
{
	public class EventPageViewModel : EventDetailModel
	{
		EventImage _selectedImage;

		public EventImage SelectedImage
		{
			get { return _selectedImage; }
			set
			{
				_selectedImage = value;
				if (_selectedImage != null)
				{
					Debug.WriteLine($"Selected image {_selectedImage.URI}");
					Application.Current.MainPage.Navigation.PushAsync(new ImageGalleryPage(ref _selectedImage, ref _eventReference));
				}
			}
		}

		// TODO: remove flot item tapped command
		public Command<ItemTappedEventArgs> FlowItemTappedCommand { get; }


		public EventPageViewModel(ref Event eventReference)
		{
			EventReference = eventReference;

			FlowItemTappedCommand = new Command<ItemTappedEventArgs>((args) =>
			{

			});
		}

	}
}
