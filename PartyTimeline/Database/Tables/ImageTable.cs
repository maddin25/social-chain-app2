using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PartyTimeline
{
	public class ImageTable : TableTemplate
	{
		private static ImageTable _instance;

		public readonly string ColumnUri = "uri";
		public readonly string ColumnCaption = "caption";
		public readonly string ColumnEventmemberId = "event_member_id";
		public readonly string ColumnEventId = "event_id";

		public static ImageTable INSTANCE
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ImageTable();
				}
				return _instance;
			}
		}

		private ImageTable()
		{
			TableName = "event_images";
			Columns.Add(new Column { Name = ColumnUri, DataType = "VARCHAR(256)", Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			Columns.Add(new Column { Name = ColumnCaption, DataType = "VARCHAR(100)" });
			Columns.Add(new Column { Name = ColumnEventmemberId, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = ColumnEventId, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL"] });

			Relationships.Add(RelationshipForeignKey(ColumnEventmemberId, EventMemberTable.INSTANCE.TableName, ColumnId));
			Relationships.Add(RelationshipForeignKey(ColumnEventId, EventTable.INSTANCE.TableName, ColumnId));
		}

		public string Insert(EventImage image, EventMember eventMember, Event eventReference)
		{
			string statement = StatementInsertInto(
				TableName,
				new Dictionary<string, object> {
				{ColumnId, image.Id},
				{ColumnUri, image.URI},
				{ColumnCaption, image.Caption},
				{ColumnDateCreated, image.DateCreated.ToFileTimeUtc()},
				{ColumnLastModified, image.DateLastModified.ToFileTimeUtc()},
				{ColumnEventmemberId, eventMember.Id},
				{ColumnEventId, eventReference.Id}
				}
			);
			return statement;
		}
	}
}
