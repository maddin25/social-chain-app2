using System;

namespace PartyTimeline
{
	public class SessionState : EventArgs
	{
		public bool IsAuthenticated { get; set; }

		public override string ToString()
		{
			return string.Format("[SessionState: IsAuthenticated={0}]", IsAuthenticated);
		}
	}
}
