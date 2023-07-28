using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAgregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending" )]
        Pending,
        [EnumMember(Value = "Payment Received" )]
        PaymentReceived,
        [EnumMember(Value = "Payment Failed" )]
        PaymentFailed
    }
}