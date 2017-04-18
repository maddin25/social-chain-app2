using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarouselView.FormsPlugin.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PartyTimeline.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGalleryPage : CarouselPage
    {
        public ImageGalleryPage(ref EventImage eventImage, ref Event eventReference)
        {
            InitializeComponent();
            Title = eventReference.Name;
            int position = FindImageIndex(eventImage, eventReference.Images);
           
            ItemsSource = eventReference.Images;
            CurrentPage = Children[position];
        }

        private int FindImageIndex(EventImage eventImage, ObservableCollection<EventImage> eventReferenceImages)
        {
            int index = 0;
            foreach (EventImage image in eventReferenceImages)
            {
                if (image.Id.Equals(eventImage.Id))
                {
                    return index;
                }
                index += 1;
            }
            return 0;
        }
    }
}
