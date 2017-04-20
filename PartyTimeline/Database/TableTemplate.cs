using System.Collections.Generic;

namespace PartyTimeline
{
	public class TableTemplate
	{
		public static List<Column> COLUMNS;
		public static string TABLE_NAME = string.Empty;

		public TableTemplate()
		{
			COLUMNS.Add(new Column { NAME = "_id", IS_PRIMARY_KEY = true, DATATYPE = DATATYPES.TEXT });
		}
	}
}
