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

		public string Name = string.Empty;
		public bool IsPrimaryKey = false;
		public string DataType;
		public string Constraint = CONSTRAINTS["NONE"];

		public string CreateColumnStatement()
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				throw new InvalidOperationException("Column NAME is null or has length 0");
			}
			else if (DataType == null)
			{
				throw new InvalidOperationException("Column DATATYPE is null");
			}

			return Name +
				" " + DataType +
				(IsPrimaryKey ? " PRIMARY KEY" : "") +
				" " + Constraint;
		}
	}
}
