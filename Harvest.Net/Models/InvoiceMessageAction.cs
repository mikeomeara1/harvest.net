using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvest.Net.Models
{
    public enum InvoiceMessageAction
    {
        [Description("unknown")]
        Unknown = 0,
        [Description("mark_as_sent")]
        MarkAsSent = 1,
        [Description("mark_as_closed")]
        MarkAsClosed = 2,
        [Description("mark_as_draft")]
        MarkAsDraft = 3,
        [Description("re_open")]
        ReOpen = 4
    }
}
