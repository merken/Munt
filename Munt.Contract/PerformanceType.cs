using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public enum PerformanceType
    {
        None = -1,
        /// <summary>
        /// A normal working business day
        /// </summary>
        BusinessDay,
        /// <summary>
        /// A normal working business day on site of the client
        /// </summary>
        BusinessDayOnSite,
        /// <summary>
        /// A day of travel (out of the country)
        /// </summary>
        BusinessTravel,
        /// <summary>
        /// Illness
        /// </summary>
        Illness,
        /// <summary>
        /// Performance in the weekend (possibly extra compensated)
        /// </summary>
        Weekend,
        /// <summary>
        /// Performance on a legal holiday (or possibly a sunday)
        /// </summary>
        WorkingHoliday,
        /// <summary>
        /// A legal payed holiday, out of office
        /// </summary>
        Holiday,
        /// <summary>
        /// A planned vacation, out of office
        /// </summary>
        Vacation
    }
}
