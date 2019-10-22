using System;
using Acrelec.Mockingbird.Interfaces.Peripherals;

namespace PaymentSenseReport
{
    public class Program
    {
      


        static void Main(string[] args)
        {
            Console.WriteLine("Starting EOD Report. ");
            Report report = new Report();
            report.EndOfDayPostRequest();
            Console.WriteLine("EOD Report finished. ");
        }
    }
}
