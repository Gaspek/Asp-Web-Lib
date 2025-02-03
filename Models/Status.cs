using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Status
    {
        public enum CopyStatus
        {
            Available,
            ReadyForPickUp,
            Borrowed,
            InQueue,
            Reserved,
            Returned
        }
    }
}