using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Domain.Enums
{
    public enum OrderStatus
    {
        WaitingForPayment = 1,
        Payed = 2,
        InPreparation = 3,
        Shipped = 4,
        OutForDelivery = 5,
        Delivered = 6,
        CancelRequested = 7,
        Canceled = 8,
        WaitingForReturnMail = 9,
        Returned = 10,
        RefundRequested = 11,
        Refunded = 12
    }
}
