using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class DataRowUtils
    {
        public static DataRow Clone(this DataRow instance)
        {
            var row = instance.Table.NewRow();
            foreach (DataColumn column in instance.Table.Columns)
            {
                row[column.ColumnName] = instance[column.ColumnName];
            }
            return row;
        }

        public static DataRow CloneAs(this DataRow instance,DataTable target)
        {
            var row = target.NewRow();            
            foreach (DataColumn column in instance.Table.Columns)
            {
                if (target.Columns.Contains(column.ColumnName))
                {
                    row[column.ColumnName] = instance[column.ColumnName];
                }
            }
            return row;
        }
    }
}
