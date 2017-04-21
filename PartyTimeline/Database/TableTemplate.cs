using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class TableTemplate
	{
		protected static string COLUMNNAME_ID = "_id";
		protected static string COLUMNDATATYPE_ID = Column.DATATYPES["INT"];

		private static string STATEMENT_CREATE_TABLE = "CREATE TABLE";
		private static string STATEMENT_DROP_TABLE = "DROP TABLE";

		public List<Column> Columns = new List<Column>();
		public List<string> Relationships = new List<string>();
		public string TableName = string.Empty;

		public TableTemplate()
		{
			AddIdColumn();
			AddDateModifiedColumn();
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
					column_create_statements += ",\n";
				}
			}

			string relationship_statements = string.Empty;
			foreach (string relationship in Relationships)
			{
				relationship_statements += ",\n\t" + relationship;
			}

			return STATEMENT_CREATE_TABLE + " " + TableName +
				" (\n" + column_create_statements + relationship_statements + "\n);";
		}

		public string DropTableQuery()
		{
			return STATEMENT_DROP_TABLE + " " + TableName + ";";
		}

		protected void AddDateModifiedColumn()
		{
			Columns.Add(new Column { Name = "date_modified", DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
		}

		protected void AddIdColumn()
		{
			Columns.Add(new Column {
				Name = COLUMNNAME_ID,
				IsPrimaryKey = true,
				DataType = COLUMNDATATYPE_ID,
				Constraint = Column.CONSTRAINTS["NOT_NULL"]
			});
		}

		protected string RelationshipForeignKey(string column_name, string reference_table, string reference_column)
		{
			return $"FOREIGN KEY({column_name}) REFERENCES {reference_table}({reference_column})";
		}
	}
}
