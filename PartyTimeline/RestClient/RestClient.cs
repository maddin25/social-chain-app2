using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PartyTimeline
{
	/// <summary>
	/// RestClient implements methods for calling CRUD operations
	/// using HTTP.
	/// </summary>
	public abstract class RestClient<T>
	{
		protected HttpClient httpClient;

		private JsonSerializerSettings serializationSettings;

		protected string endpoint = string.Empty;

		protected const string serverBaseUrl = "http://lowcost-env.zk8xjtydiz.us-west-2.elasticbeanstalk.com";
		protected const string appName = "partytimeline";
		protected const string appApiNode = "api";
		protected const string apiVersion = "v1";
		protected string serverUrl;

		public RestClient(string endpoint)
		{
			this.endpoint = endpoint;
			serverUrl = string.Join("/", serverBaseUrl, appName, appApiNode, apiVersion, this.endpoint);
			Debug.WriteLine($"Using server URL {serverUrl} for type {this.GetType().ToString()}");
			httpClient = new HttpClient();
			serializationSettings = new JsonSerializerSettings()
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				DateFormatString = "yyyy-MM-dd HH:mm:ss"
			};
		}

		public async Task<List<T>> GetAsync()
		{
			var json = await httpClient.GetStringAsync(serverUrl);

			var taskModels = JsonConvert.DeserializeObject<List<T>>(json, serializationSettings);

			return taskModels;
		}

		public async Task<bool> PostAsync(T t)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);
			Debug.WriteLine($"POST: Serialized object for {this.GetType().ToString()}:\n{json}");
			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PostAsync(serverUrl, httpContent);
			Debug.WriteLine($"POST result:\n\tStatusCode: {result.StatusCode}\n\tRequestMessage: {result.RequestMessage.ToString()}");
			return result.IsSuccessStatusCode;
		}

		public async Task<bool> PutAsync(int id, T t)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);

			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PutAsync(string.Join("/", serverUrl, id), httpContent);

			return result.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteAsync(int id, T t)
		{
			var response = await httpClient.DeleteAsync(string.Join("/", serverUrl, id));

			return response.IsSuccessStatusCode;
		}
	}
}
