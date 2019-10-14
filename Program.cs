using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentSenseReport
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running End of Day Report.\n");
            Report report = new Report();
            report.EndOfDayPostRequest();

            Console.WriteLine("Report finished.\nCheck reports folder for report.");
            Thread.Sleep(2000);

        }
    }
}
