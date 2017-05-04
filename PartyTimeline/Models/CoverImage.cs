using System;

using Newtonsoft.Json;

namespace PartyTimeline
{
	public class CoverImage
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("source")]
		public Uri Source { get; set; }
	}
}
