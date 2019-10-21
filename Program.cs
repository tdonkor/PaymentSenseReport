using System;
using System.Threading;
using Acrelec.Library.Logger;

namespace PaymentSenseReport
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Report. ");
            Report report = new Report();
            report.EndOfDayPostRequest();
            Console.WriteLine("Report finished. ");

        }
    }
}
