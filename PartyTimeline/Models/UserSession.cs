using System;

using Newtonsoft.Json;

using Plugin.DeviceInfo;

namespace PartyTimeline
{
	public class UserSession
	{
        public const string UidEventMemberId = "event_member_id";
        public const string UidExpiresOn = "expires_on";
        public const string UidDeviceOsVersion = "device_os_version";
        public const string UidDevicePlatform = "device_platform";
        public const string UidDeviceModel = "device_model";

        [JsonProperty(BaseModel.UidId)]
		public string Id { get; set; }

		[JsonProperty(UidEventMemberId)]
		public long EventMemberId { get; set; }

		[JsonProperty(UidExpiresOn)]
		public DateTime ExpiresOn { get; set; }

		[JsonProperty(UidDeviceOsVersion)]
		public string DeviceOsVersion => CrossDeviceInfo.Current.Version;

	    [JsonProperty(UidDevicePlatform)]
		public string DevicePlatform => CrossDeviceInfo.Current.Platform.ToString();

	    [JsonProperty(UidDeviceModel)]
		public string DeviceModel => CrossDeviceInfo.Current.Model;

	    public override string ToString()
	    {
            return JsonConvert.SerializeObject(this);
	    }
	}
}
