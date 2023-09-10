using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.Entities
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateTime date)
        {
            var today =DateTime.UtcNow;
            var age=today.Year-date.Year;
            if (date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
