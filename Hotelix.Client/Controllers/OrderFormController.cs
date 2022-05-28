using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models;
using Hotelix.Client.Models.Api;
using Hotelix.Client.Models.Database;
using Hotelix.Client.Services;
using Hotelix.Client.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Client.Controllers
{
    public class OrderFormController : Controller
    {
        private readonly OrderForm _orderForm;
        private readonly IOfferService _offerService;

        public OrderFormController(OrderForm orderForm, IOfferService offerService)
        {
            _orderForm = orderForm;
            _offerService = offerService;
        }

        public ViewResult Index()
        {
            /*HttpContext.Session.SetInt32("OrderForm.RoomId", 12);
            HttpContext.Session.SetString("OrderForm.StartTime", "2021-12-01");
            HttpContext.Session.SetString("OrderForm.EndTime", "2021-12-09");*/
            
            /*return SetAlertCookieAndRedirect(new AlertModel()
            {
                Type = "danger",
                Message = "" + _orderForm.GetRoom() + ", " +
                          _orderForm.GetStartTime() + ", " +
                          _orderForm.GetEndTime()
            }, "Index", "Home");*/
            
            /*return SetAlertCookieAndRedirect(new AlertModel()
            {
                Type = "danger",
                Message = "" + HttpContext.Session.GetInt32("OrderForm.RoomId") + ", " +
                          HttpContext.Session.GetString("OrderForm.StartTime") + ", " +
                          HttpContext.Session.GetString("OrderForm.EndTime")
            }, "Index", "Home");*/
            
            
            
            return View(new OrderFormViewModel
            {
                OrderForm = _orderForm.IsValid() ? _orderForm : null,
                TotalPrice = _orderForm.GetTotalPrice()
            });
        }

        public async Task<RedirectToActionResult> AddRoomToOrderForm(int roomId, DateTime startTime, DateTime endTime)
        {
            Room room;

            try
            {
                room = await _offerService.GetRoom(roomId);
            }
            catch (NotFoundException)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrany pokój nie istnieje."
                }, "Index");
            }

            _orderForm.SetOrderForm(room, startTime, endTime);
            
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveRoomFromOrderForm(int roomId)
        {
            _orderForm.ClearOrder();
            
            return RedirectToAction("Index");
        }



        public void SetAlertCookie(AlertModel alert)
        {
            HttpContext.Response.Cookies.Append("alertType", alert.Type);
            HttpContext.Response.Cookies.Append("alertMessage", alert.Message);
        }
        
        public RedirectToActionResult SetAlertCookieAndRedirect(AlertModel alert, string actionName,
            string controllerName = null, object routeValues = null, string fragment = null)
        {
            SetAlertCookie(alert);
            return RedirectToAction(actionName, controllerName, routeValues, fragment);
        }
    }
}
