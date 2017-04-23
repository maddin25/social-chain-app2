using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class TableTemplate
	{
		public readonly string ColumnId = "_id";
		public readonly string ColumnLastModified = "date_modified";

		public readonly string ColumnDatatypeId = Column.DATATYPES["INT"];

		private readonly string STATEMENT_CREATE_TABLE = "CREATE TABLE";
		private readonly string STATEMENT_DROP_TABLE = "DROP TABLE";

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
				// TODO: use string.Join
				column_create_statements += "\t" + column.CreateColumnStatement();
				if (column != Columns[Columns.Count - 1])
				{
					column_create_statements += ",\n";
				}
			}

			string relationship_statements = string.Empty;
			foreach (string relationship in Relationships)
			{
				// TODO: use string.Join
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
			Columns.Add(new Column { Name = ColumnLastModified, DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
		}

		protected void AddIdColumn()
		{
			Columns.Add(new Column
			{
				Name = ColumnId,
				IsPrimaryKey = true,
				DataType = ColumnDatatypeId,
				Constraint = Column.CONSTRAINTS["NOT_NULL"]
			});
		}

		protected string RelationshipForeignKey(string column_name, string reference_table, string reference_column)
		{
			return $"FOREIGN KEY({column_name}) REFERENCES {reference_table}({reference_column})";
		}

		protected string StatementInsertInto(string table, Dictionary<string, Object> column_value_pairs)
		{
			Debug.Assert(column_value_pairs.ContainsKey(ColumnId), $"Values for column {ColumnLastModified} must be inserted");
			Debug.Assert(column_value_pairs.ContainsKey(ColumnLastModified), $"Values for column {ColumnLastModified} must be inserted");

			return $"INSERT INTO {table}({string.Join(", ", column_value_pairs.Keys)}) VALUES ({string.Join(", ", column_value_pairs.Values)});";
		}
	}
}
