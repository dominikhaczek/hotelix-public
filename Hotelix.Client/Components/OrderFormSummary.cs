using Hotelix.Client.Models;
using Hotelix.Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Client.Components
{

    public class OrderFormSummary : ViewComponent
    {
        private readonly OrderForm _orderForm;

        public OrderFormSummary(OrderForm orderForm)
        {
            _orderForm = orderForm;
        }

        public IViewComponentResult Invoke()
        {
            return View(new OrderFormViewModel
            {
                OrderForm = _orderForm.IsValid() ? _orderForm : null,
                TotalPrice = _orderForm.GetTotalPrice()
            });
        }
    }
}
