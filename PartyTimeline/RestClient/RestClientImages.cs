using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class RestClientImages : RestClient<EventImage>
	{
		public RestClientImages() : base("event_image")
		{

		}

		public async Task<bool> UploadImage(EventImage image, string quality)
		{
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
			httpContent.Add(new StringContent(image.EventMemberId.ToString()), EventImage.UidEventMemberId);
			httpContent.Add(new StringContent(quality), ImageQualities.UidImageQuality);

			Debug.WriteLine($"POST {nameof(EventImage)}:\n\t{httpContent.ToString()}");
			var result = await httpClient.PostAsync(UrlJoin(serverUrl, "upload"), httpContent);
			return result.IsSuccessStatusCode;
		}
	}
}
