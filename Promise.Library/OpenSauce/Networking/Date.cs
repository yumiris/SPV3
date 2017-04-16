using System;

namespace Promise.Library.OpenSauce.Networking
{
    public class Date
    {
        public int Day { get; set; } = DateTime.Today.Day;
        public int Month { get; set; } = DateTime.Today.Month;
        public int Year { get; set; } = DateTime.Today.Year;
    }
}