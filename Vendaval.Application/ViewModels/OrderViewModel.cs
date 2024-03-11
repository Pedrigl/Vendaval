using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;
using Vendaval.Domain.Enums;
using Vendaval.Domain.ValueObjects;

namespace Vendaval.Application.ViewModels
{
    public class OrderViewModel : BaseModelViewModel
    {
        public int CustomerId { get; set; }
        public required List<ProductViewModel> Products { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int Installments { get; set; }
        public decimal InstallmentValue { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Address? DeliveryAddress { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string? TrackingCode { get; set; }
        public string? OrderNotes { get; set; }
    }
}
