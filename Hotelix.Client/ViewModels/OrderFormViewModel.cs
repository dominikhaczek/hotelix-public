using System.Collections.Generic;
using Hotelix.Client.Models;
using Hotelix.Client.Models.Database;

namespace Hotelix.Client.ViewModels
{
    public class OrderFormViewModel
    {
        public OrderForm OrderForm { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
