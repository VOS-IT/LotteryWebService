using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryWebService
{
    public class UserHistory
    {
        public string TransactionId { get; set; }
        public string TicketNo { get; set; }
        public string UserId { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }

    }
}