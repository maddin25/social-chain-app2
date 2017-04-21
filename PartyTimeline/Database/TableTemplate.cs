using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class TableTemplate
	{
		private static string STATEMENT_CREATE_TABLE = "CREATE TABLE";

		public List<Column> COLUMNS = new List<Column>();
		public string TABLE_NAME = string.Empty;

		public TableTemplate()
		{
			COLUMNS.Add(new Column {
				NAME = "_id",
				IS_PRIMARY_KEY = true,
				DATATYPE = Column.DATATYPES["INT"],
				CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"]
			});
		}

		public string CreateTableQuery()
		{
			if (string.IsNullOrWhiteSpace(TABLE_NAME))
			{
				throw new InvalidOperationException("TABLE_NAME is null or only white space");
			}

			string column_create_statements = string.Empty;
			foreach (Column column in COLUMNS)
			{
				column_create_statements += "\t" + column.CreateColumnStatement();
				if (column != COLUMNS[COLUMNS.Count - 1])
				{
					column_create_statements += ",";
				}
				column_create_statements += "\n";
			}

			return STATEMENT_CREATE_TABLE + " " + TABLE_NAME +
				" (\n" + column_create_statements + ")";
		}
	}
}
