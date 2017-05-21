using System;

using Newtonsoft.Json;

using Plugin.DeviceInfo;

namespace PartyTimeline
{
	public class UserSession
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("event_member_id")]
		public long EventMemberId { get; set; }

		[JsonProperty("expires_on")]
		public DateTime ExpiresOn { get; set; }

		[JsonProperty("device_os_version")]
		public string DeviceOsVersion
		{
			get { return CrossDeviceInfo.Current.Version; }
		}

		[JsonProperty("device_platform")]
		public string DevicePlatform
		{
			get { return CrossDeviceInfo.Current.Platform.ToString(); }
		}

		[JsonProperty("device_model")]
		public string DeviceModel
		{
			get { return CrossDeviceInfo.Current.Model; }
		}
	}
}
