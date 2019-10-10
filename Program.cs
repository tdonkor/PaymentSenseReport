using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSenseReport
{
    public class Program
    {
        static void Main(string[] args)
        {
            Report report = new Report();
            report.EndOfDayPostRequest();
            Console.WriteLine("Press any key to exit...");
        }
    }
}
