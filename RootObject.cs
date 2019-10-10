using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSenseReport
{

    public class RootObject
    {
        public Balances balances { get; set; }
        public Banking banking { get; set; }
        public List<ReportLine> reportLines { get; set; }
        public DateTime reportTime { get; set; }
        public string reportType { get; set; }
        public string reportResult { get; set; }
        public string tid { get; set; }
        public string requestId { get; set; }
        public string location { get; set; }
        public List<List<string>> notifications { get; set; }
    }

    public class Balances
    {
        public string currency { get; set; }
        public IssuerTotals issuerTotals { get; set; }
        public string totalAmount { get; set; }
        public string totalCashbackAmount { get; set; }
        public string totalCashbackCount { get; set; }
        public string totalGratuityAmount { get; set; }
        public string totalGratuityCount { get; set; }
        public string totalRefundsAmount { get; set; }
        public string totalRefundsCount { get; set; }
        public string totalSalesAmount { get; set; }
        public string totalSalesCount { get; set; }
        public string totalsSince { get; set; }
        public WaiterTotals waiterTotals { get; set; }
    }

    public class IssuerTotals
    {
        public IssuerName issName { get; set; }
    }

    public class IssuerName
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }



    public class WaiterId
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }

    public class WaiterTotals
    {
        public WaiterId waiterId { get; set; }
    }



    public class IssuerName2
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }

    public class CurrentSessionIssuerTotals
    {
        public IssuerName2 issName { get; set; }
    }

    public class IssuerName3
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }

    public class PreviousSessionIssuerTotals
    {
        public IssuerName3 issName { get; set; }
    }

    public class CurrentSessionTotals
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }

    public class PreviousSessionTotals
    {
        public string currency { get; set; }
        public int totalAmount { get; set; }
        public int totalRefundsAmount { get; set; }
        public int totalRefundsCount { get; set; }
        public int totalSalesAmount { get; set; }
        public int totalSalesCount { get; set; }
    }

    public class AcquirerName
    {
        public string currency { get; set; }
        public CurrentSessionIssuerTotals currentSessionIssuerTotals { get; set; }
        public PreviousSessionIssuerTotals previousSessionIssuerTotals { get; set; }
        public CurrentSessionTotals currentSessionTotals { get; set; }
        public PreviousSessionTotals previousSessionTotals { get; set; }
        public List<string> currentSessionTransactionNumbers { get; set; }
        public List<string> previousSessionTransactionNumbers { get; set; }
    }

    public class Banking
    {
        public AcquirerName acquirerName { get; set; }
    }

    public class ReportLine
    {
        public List<string> format { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}




