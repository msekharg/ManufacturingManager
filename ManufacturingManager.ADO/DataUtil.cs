using System.Data;

namespace ManufacturingManager.ADO
{
    public class DataUtil
    {


        public static int? GetNullableInt(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull) ? null : Convert.ToInt32(GetValue(dr, columnName));
        }

        public static double? GetNullableDouble(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull)
                ? null
                : Convert.ToDouble(GetValue(dr, columnName));
        }
        public static double GetNullableDoubleAsZero(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull)
                ? 0
                : Convert.ToDouble(GetValue(dr, columnName));
        }

        public static decimal? GetNullableDecimal(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull)
                ? null
                : Convert.ToDecimal(GetValue(dr, columnName));
        }
        public static bool? GetNullableBool(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull) ? null : Convert.ToBoolean(GetValue(dr, columnName));
        }

        public static DateTime? GetNullableDateTime(DataRow dr, string columnName)
        {
            return (GetValue(dr, columnName) is DBNull)
                ? null
                : Convert.ToDateTime(GetValue(dr, columnName));
        }

        private static object GetValue(DataRow dr, string columnName)
        {
            return dr[columnName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static double GetDouble(DataRow dr, string columnName)
        {
            if (dr[columnName] is DBNull)
            {
             
                return -1;
            }

            if (!double.TryParse(dr[columnName].ToString(), out var value))
            {
                return -1;
            }

            return value;
        }


    }
}
