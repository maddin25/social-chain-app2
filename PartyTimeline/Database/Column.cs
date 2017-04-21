using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class Column
	{
		public static Dictionary<string, string> DATATYPES = new Dictionary<string, string>
		{
			{"INT", "INTEGER"},
			{"REAL", "REAL"},
			{"TEXT", "TEXT"}
		};

		public static Dictionary<string, string> CONSTRAINTS = new Dictionary<string, string>
		{
			{"NOT_NULL", "NOT NULL"},
			{"UNIQUE", "UNIQUE"},
			{"NOT_NULL_UNIQUE", "NOT NULL UNIQUE"},
			{"NONE", ""}
			// Missing: CHECK and DEFAULT constraints
		};

		public string NAME = string.Empty;
		public bool IS_PRIMARY_KEY = false;
		public string DATATYPE;
		public string CONSTRAINT = CONSTRAINTS["NONE"];

		public string CreateColumnStatement()
		{
			if (string.IsNullOrWhiteSpace(NAME))
			{
				throw new InvalidOperationException("Column NAME is null or has length 0");
			}
			else if (DATATYPE == null)
			{
				throw new InvalidOperationException("Column DATATYPE is null");
			}

			return NAME +
				" " + DATATYPE +
				(IS_PRIMARY_KEY ? " PRIMARY KEY" : "") +
				" " + CONSTRAINT;
		}
	}
}
