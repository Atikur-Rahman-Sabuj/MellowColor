using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class OrderDataAccess : GenericDataAccess<Order>
    {
        public List<Order> GetByDate(DateTime date)
        {
            return _mellowColorDbSet.Where(a => DbFunctions.TruncateTime(a.Date) == date.Date).AsEnumerable().ToList();
        }
        public List<Order> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            return _mellowColorDbSet.Where(a => DbFunctions.TruncateTime(a.Date) >= fromDate.Date && DbFunctions.TruncateTime(a.Date) <= toDate.Date).AsEnumerable().ToList();
        }
    }
}
