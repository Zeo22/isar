using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAR.Extensions
{
    public static class ClassExtensions
    {
        public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
    }
}