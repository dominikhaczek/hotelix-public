using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models;
using Hotelix.Client.Models.Api;
using Hotelix.Client.Services;
using Hotelix.Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Hotelix.Client.Controllers
{
    [Authorize]
    public class ReservationManagementController : Controller
    {
        private readonly IReservationsService _reservationsService;
        private readonly OrderForm _orderForm;

        public ReservationManagementController(IReservationsService reservationsService, OrderForm orderForm)
        {
            _reservationsService = reservationsService;
            _orderForm = orderForm;
        }
        
        public async Task<IActionResult> Index()
        {
            /*_orderForm.SetRoomId(50);
            _orderForm.SetStartTime("2021-11-21");
            _orderForm.SetEndTime("2021-11-29");*/
            //_orderForm.SetOrderForm(new Room(), DateTime.Today, DateTime.Today.AddDays(2));
            /*session.SetInt32("OrderForm.RoomId", 34);
            session.SetString("OrderForm.StartTime", "2021-12-21");
            session.SetString("OrderForm.EndTime", "2021-12-29");*/
            var reservations = await _reservationsService.GetAllReservations();

            if (User.IsInRole("Klient"))
            {
                var userId = User.FindFirst(c => c.Type == "sub")?.Value;
                reservations = reservations.Where(r => r.UserId == userId);
            }

            foreach (var r in reservations)
            {
                r.PricePerNight *= Convert.ToDecimal((r.EndTime - r.StartTime).TotalDays);
            }

            return View(reservations);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            /*return SetAlertCookieAndRedirect(new AlertModel
            {
                Type = "danger",
                Message = "clientId = " + HttpContext.Session.GetInt32("clientId")
            }, "Index");*/
            
            Reservation reservation;
            
            try
            {
                reservation = await _reservationsService.GetReservation(id);
            }
            catch (NotFoundException)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrana rezerwacja nie istnieje."
                }, "Index");
            }
            
            if (User.IsInRole("Klient") && reservation.UserId != User.FindFirst(c => c.Type == "sub")?.Value)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Nie masz uprawnień, żeby wykonać tę operację."
                }, "Index");
            }

            reservation.PricePerNight *= Convert.ToDecimal((reservation.EndTime - reservation.StartTime).TotalDays);

            return View(reservation);
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            Reservation reservation;
            
            try
            {
                reservation = await _reservationsService.GetReservation(id);
            }
            catch (NotFoundException)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrana rezerwacja nie istnieje."
                }, "Index");
            }
            
            if (User.IsInRole("Klient") && reservation.UserId != User.FindFirst(c => c.Type == "sub")?.Value)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Nie masz uprawnień, żeby wykonać tę operację."
                }, "Index");
            }
            
            // no need to handle NotFoundException - already handled above in GetReservation()
            await _reservationsService.RemoveReservation(id);
            return SetAlertCookieAndRedirect(new AlertModel
            {
                Type = "success",
                Message = "Pomyślnie usunięto rezerwację."
            }, "Index");
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
