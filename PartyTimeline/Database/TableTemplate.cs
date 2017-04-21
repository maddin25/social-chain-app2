using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class TableTemplate
	{
		private static string STATEMENT_CREATE_TABLE = "CREATE TABLE";
		private static string STATEMENT_DROP_TABLE = "DROP TABLE";

		public List<Column> Columns = new List<Column>();
		public string TableName = string.Empty;

		public TableTemplate()
		{
			Columns.Add(new Column {
				Name = "_id",
				IsPrimaryKey = true,
				DataType = Column.DATATYPES["INT"],
				Constraint = Column.CONSTRAINTS["NOT_NULL"]
			});
		}

		public string CreateTableQuery()
		{
			if (string.IsNullOrWhiteSpace(TableName))
			{
				throw new InvalidOperationException("TABLE_NAME is null or only white space");
			}

			string column_create_statements = string.Empty;
			foreach (Column column in Columns)
			{
				column_create_statements += "\t" + column.CreateColumnStatement();
				if (column != Columns[Columns.Count - 1])
				{
					column_create_statements += ",";
				}
				column_create_statements += "\n";
			}

			return STATEMENT_CREATE_TABLE + " " + TableName +
				" (\n" + column_create_statements + ");";
		}

		public string DropTableQuery()
		{
			return STATEMENT_DROP_TABLE + " " + TableName + ";";
		}
	}
}
