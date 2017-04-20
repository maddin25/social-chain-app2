using System;

namespace PartyTimeline
{
	public enum DATATYPES { INT, REAL, TEXT };
	public enum CONSTRAINTS { NOT_NULL, UNIQUE, NONE, PRIMARY };

	public class Column
	{
		public string NAME;
		public bool IS_PRIMARY_KEY = false;
		public DATATYPES DATATYPE;
		public CONSTRAINTS CONSTRAINT = CONSTRAINTS.NONE;
	}
}
