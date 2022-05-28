using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models;
using Hotelix.Client.Models.Api;
using Hotelix.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotelix.Client.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly OrderForm _orderForm;
        private readonly IReservationsService _reservationsService;

        public ReservationController(OrderForm orderForm, IReservationsService reservationsService)
        {
            _orderForm = orderForm;
            _reservationsService = reservationsService;
        }

        public async Task<IActionResult> Checkout()
        {
            if (!_orderForm.IsValid())
            {
                ModelState.AddModelError("", "Formularz rezerwacji jest pusty.");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(c => c.Type == "sub")?.Value;

                int result;

                try
                {
                    result = await _reservationsService.CreateReservation(new ReservationToCreate()
                    {
                        UserId = userId,
                        RoomId = _orderForm.GetRoom().Id,
                        StartTime = _orderForm.GetStartTime(),
                        EndTime = _orderForm.GetEndTime()
                    });
                }
                catch (Exception e) when (
                    e is BadRequestException ||
                    e is NotFoundException ||
                    e is MethodNotAllowedException ||
                    e is ServiceUnavailableException
                    )
                {
                    if (!(e is ServiceUnavailableException))
                    {
                        _orderForm.ClearOrder();
                    }
                    
                    var message = e switch
                    {
                        BadRequestException _ => "Wybrany termin rezerwacji nie jest poprawny.",
                        NotFoundException _ => "Ups, coś poszło nie tak - wygląda na to, że wybrane przez Ciebie komponenty rezerwacji nie istnieją.",
                        MethodNotAllowedException _ => "Rezerwacja wybranego pokoju nie jest już możliwa w podanym terminie - ktoś zrobił to wcześniej lub pokój właśnie został wycofany z oferty.",
                        ServiceUnavailableException _ => "Serwer nie mógł obsłużyć żądania, spróbuj ponownie za chwilę.",
                        _ => null
                    };
                    
                    return SetAlertCookieAndRedirect(new AlertModel
                    {
                        Type = "danger",
                        Message = message
                    }, "Index", "OrderForm");
                }

                if (result != 0)
                {
                    _orderForm.ClearOrder();
                    return RedirectToAction("CheckoutComplete");
                }
            }

            return RedirectToAction("Index", "OrderForm");
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMsg = "Dziękujemy za twoją rezerwację!";

            return View();
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
