using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		protected JsonSerializerSettings serializationSettings;

		protected string endpoint = string.Empty;

        protected const string sep = "/";
        protected const string RequestGet = "GET";
        protected const string RequestPost = "POST";

        protected const string serverBaseUrl = "http://lowcost-env.zk8xjtydiz.us-west-2.elasticbeanstalk.com";
		protected const string appName = "partytimeline";
		protected const string appApiNode = "api";
		protected const string apiVersion = "v1";
		protected string serverUrl;

		public RestClient(string endpoint)
		{
			this.endpoint = endpoint;
			serverUrl = UrlJoin(serverBaseUrl, appName, appApiNode, apiVersion, endpoint);
			Debug.WriteLine($"Using server URL {serverUrl} for type {this.GetType().ToString()}");
			httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serverUrl);
            //httpClient.Timeout = new TimeSpan(0, 0, 3); // [h, m, s]

            //httpClient.DefaultRequestHeaders.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			serializationSettings = new JsonSerializerSettings()
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatString = "yyyy-MM-dd'T'HH:mm:ss'Z'"
            };
		}

		public async Task<List<T>> GetAsync(string custom_endpoint = null)
		{
			var json = await httpClient.GetStringAsync(custom_endpoint);

			var taskModels = JsonConvert.DeserializeObject<List<T>>(json, serializationSettings);

			return taskModels;
		}

		public async Task<bool> PostAsync(T t, string custom_endpoint = null)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);
			Debug.WriteLine($"POST: Serialized object for {this.GetType().ToString()}: {json}");
			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PostAsync(custom_endpoint, httpContent);
            LogResponse(result);
			return result.IsSuccessStatusCode;
		}

		public async Task<bool> PutAsync(int id, T t, string custom_endpoint = null)
		{
			var json = JsonConvert.SerializeObject(t, serializationSettings);

			HttpContent httpContent = new StringContent(json);

			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			var result = await httpClient.PutAsync(UrlJoin(custom_endpoint, id), httpContent);
            LogResponse(result);
            return result.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteAsync(int id, T t, string custom_endpoint = null)
		{
			var result = await httpClient.DeleteAsync(UrlJoin(custom_endpoint, id));
            LogResponse(result);

            return result.IsSuccessStatusCode;
        }

        public static string UrlJoin(params object[] parts)
		{
			return string.Join(sep, parts.Where((arg) => arg != null).Select<object, string>((part) =>
            {
                string s = part.ToString();
                s = s.TrimStart(sep.ToCharArray());
                s = s.TrimEnd(sep.ToCharArray());
                    
                return s;
            }
            ));
		}

        protected async void LogResponse(HttpResponseMessage msg)
        {
            string log_sep = "\n#####\n";
            Debug.WriteLine($"{log_sep}Response:\nStatusCode = {msg.StatusCode}\nRequestMessage = {msg.RequestMessage.ToString().Replace("\n", " ")}\nContent = {await msg.Content.ReadAsStringAsync()}{log_sep}");
        }

        /// <summary>
        /// Builds an html query string.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected string BuildHttpQuery(params string[] args)
        {
            int nargs = args.Length;
            if (nargs % 2 != 0)
            {
                throw new ArgumentException("An even number of arguments needs to be passed");
            }
            if (args.Length == 0)
            {
                throw new ArgumentException("No query content provided");
            }
            
            List<string> parts = new List<string>(nargs / 2);
            for (int i = 0; i < nargs; i += 2)
            {
                parts.Add($"{args[i]}={args[i + 1]}");
            }
            return "?" + string.Join("&", parts);
        }
	}
}
