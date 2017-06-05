using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Xamarin.Auth;
using Xamarin.Forms;

namespace PartyTimeline
{
	public class RestClientImages : RestClient<EventImage>
	{
        const string EndpointEventImage = "event_image";
        const string EndpointEventImages = "event_images";
        const string EndpointUpload = "upload";

        public RestClientImages() : base(EndpointEventImage)
		{

		}

		public async Task<bool> UploadImage(EventImage image, string quality)
		{
            Debug.WriteLine($"{nameof(UploadImage)}: uploading {nameof(EventImage)} with '{EventImage.UidId}': {image.Id}");

            string path;
			switch (quality)
			{
				case ImageQualities.SMALL:
					path = image.PathSmall;
					break;
				case ImageQualities.ORIGINAL:
					path = image.PathOriginal;
					break;
				default:
					Debug.WriteLine($"Unknown quality identifier: {quality}");
					return false;
			}
			var httpContent = new MultipartFormDataContent();
			HttpContent imageContent = new StreamContent( new MemoryStream(
				DependencyService.Get<SystemInterface>().ReadFile(path)
			));
			httpContent.Add(imageContent, "event_image_file", Path.GetFileName(path));
			httpContent.Add(new StringContent(image.Id.ToString()), BaseModel.UidId);
			httpContent.Add(new StringContent(image.EventId.ToString()), EventImage.UidEventId);
			httpContent.Add(new StringContent("10206756772397816"), EventImage.UidEventMemberId);
			httpContent.Add(new StringContent(quality), ImageQualities.UidImageQuality);
            
			var response = await httpClient.PostAsync(UrlJoin(EndpointEventImage, EndpointUpload), httpContent);
            LogResponse(response);
			return response.IsSuccessStatusCode;
		}

        public async Task<List<EventImage>> GetEventImages(long eventId)
        {
            HttpResponseMessage response = await httpClient.GetAsync(EndpointEventImages + BuildHttpQuery(
                EventImage.UidEventId, eventId.ToString(),
                "projection", "InLineEventImage"
                ));
            LogResponse(response);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"ERROR: Failed getting the event images for event with ID {eventId}");
                return null;
            }
            var images_list = JObject.Parse(await response.Content.ReadAsStringAsync())["_embedded"]["event_images"];

            List<EventImage> event_images = JsonConvert.DeserializeObject<List<EventImage>>(images_list.ToString(), serializationSettings);
            return event_images;
        }
	}
}
