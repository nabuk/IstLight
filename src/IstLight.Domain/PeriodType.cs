using System;

namespace IstLight.Domain
{
    public enum PeriodType : int
    {
        Tick,
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month
    }

    public static class PeriodTimespan
    {
        public static TimeSpan ToTimeSpan(this PeriodType period)
        {
            switch (period)
            {
                case PeriodType.Tick: return TimeSpan.FromTicks(1);
                case PeriodType.Second: return TimeSpan.FromSeconds(1);
                case PeriodType.Minute: return TimeSpan.FromMinutes(1);
                case PeriodType.Hour: return TimeSpan.FromHours(1);
                case PeriodType.Day: return TimeSpan.FromDays(1);
                case PeriodType.Week: return TimeSpan.FromDays(7);
                case PeriodType.Month: return TimeSpan.FromDays(30);
                default: throw new NotImplementedException();
            }
        }
    }
}
