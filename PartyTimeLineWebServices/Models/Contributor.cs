using System;
namespace PartyTimeLineWebServices.Models
{
	public class Contributor
	{
	    public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FacebookToken { get; set; }

		public Contributor()
		{
		}
	}
}
