﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LotteryWebService
{
    public class AmountInfo
    {
        public float ReedemCost { get; set; }
        public float AccountCost { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }

    }
}