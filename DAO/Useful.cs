using System;

namespace DAO
{
    public class Useful
    {
        public static object ValidateNull(object value)
        {
            if (value == null)
                return DBNull.Value;

            return value;
        }

        public static object ValidateNullFromDb(object value)
        {
            return (value == DBNull.Value) ? null : value;
        }
    }
}