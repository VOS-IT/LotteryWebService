using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace LotteryWebService
{
    public class TicketInfo
    {
        public int TicketCount{get;set;}
        public String TicketNo { get; set; }
        public int PriceAmount { get; set; }
        public int TicketPrice { get; set; }
        public DateTime CloseDate { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }

    }
}