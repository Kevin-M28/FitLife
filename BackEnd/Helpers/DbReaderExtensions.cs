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
            return !reader.IsDBNull(reader.GetOrdinal(column)) ? reader.GetString(reader.GetOrdinal(column)) : null;
        }

        public static int TryGetInt(this DbDataReader reader, string column)
        {
            return !reader.IsDBNull(reader.GetOrdinal(column)) ? reader.GetInt32(reader.GetOrdinal(column)) : 0;
        }

        public static decimal TryGetDecimal(this DbDataReader reader, string column)
        {
            return !reader.IsDBNull(reader.GetOrdinal(column)) ? reader.GetDecimal(reader.GetOrdinal(column)) : 0;
        }

        public static DateTime? TryGetDate(this DbDataReader reader, string column)
        {
            return !reader.IsDBNull(reader.GetOrdinal(column)) ? reader.GetDateTime(reader.GetOrdinal(column)) : (DateTime?)null;
        }
    }

}
