using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Helpers
{
    public static class DbReaderExtensions
    {
        public static string TryGetString(this DbDataReader reader, string column)
        {
            try
            {
                int index = reader.GetOrdinal(column);
                return !reader.IsDBNull(index) ? reader.GetString(index) : null;
            }
            catch
            {
                return null;
            }
        }

        public static int TryGetInt(this DbDataReader reader, string column)
        {
            try
            {
                int index = reader.GetOrdinal(column);
                return !reader.IsDBNull(index) ? Convert.ToInt32(reader.GetValue(index)) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal TryGetDecimal(this DbDataReader reader, string column)
        {
            try
            {
                int index = reader.GetOrdinal(column);
                return !reader.IsDBNull(index) ? Convert.ToDecimal(reader.GetValue(index)) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime? TryGetDate(this DbDataReader reader, string column)
        {
            try
            {
                int index = reader.GetOrdinal(column);
                return !reader.IsDBNull(index) ? Convert.ToDateTime(reader.GetValue(index)) : (DateTime?)null;
            }
            catch
            {
                return null;
            }
        }
    }

}
