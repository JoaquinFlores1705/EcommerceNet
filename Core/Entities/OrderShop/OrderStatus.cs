using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderShop
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pendiente")]
        Pending,
        [EnumMember(Value = "El pago fue recibido")]
        PaymentReceived,
        [EnumMember(Value = "El Pago tuvo errores")]
        PaymentFailure
    }
}
