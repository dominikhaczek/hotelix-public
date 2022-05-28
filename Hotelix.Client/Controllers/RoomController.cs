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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotelix.Client.Controllers
{
    public class RoomController : Controller
    {
        private readonly IOfferService _offerService;

        public RoomController( IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> List(RoomListViewModel roomListViewModel)
        {
            IEnumerable<Room> rooms; 
            int currentLocation;
            DateTime startTime;
            DateTime endTime;

            List<SelectListItem> locations = (await _offerService.GetAllLocations()).Select(l => 
                new SelectListItem { Value = l.Id.ToString(), Text = l.Name }).ToList();
            locations.Insert(0, new SelectListItem { Value = "0", Text = "Wszystkie" });

            ViewBag.LocationList = locations;

            // Ustaw defaultowa date na dzisiaj
            startTime = roomListViewModel.StartTime == DateTime.MinValue ? DateTime.Today : roomListViewModel.StartTime;
            endTime = roomListViewModel.EndTime == DateTime.MinValue ? DateTime.Today.AddDays(1) : roomListViewModel.EndTime;

            if (roomListViewModel.CurrentLocation == 0 || roomListViewModel.CurrentLocation == null)//string.IsNullOrEmpty(roomListViewModel.CurrentLocation) || roomListViewModel.CurrentLocation == "0")
            {
                currentLocation = 0;
            }
            else
            {
                currentLocation = (await _offerService.GetAllLocations()).FirstOrDefault(c => c.Id == roomListViewModel.CurrentLocation)!.Id;
            }

            // TODO: HOTELIX - GET rooms available for selected date and category. wyjasnialm: availability check for given date done in Offer and Reservations services so I think all done about this TODO? selection for given location also done below?
            if (currentLocation == 0)
            {
                rooms = await _offerService.GetAllRooms();  //.GetCarsAvailableForSelectedDate(startTime, endTime, currentCategory);
            }
            else
            {
                rooms = await _offerService.GetRoomsByLocationId(currentLocation);  //.GetCarsAvailableForSelectedDate(startTime, endTime, currentCategory);
            }
            
            try
            {
                rooms = await _offerService.FilterAvailableRooms(rooms, startTime, endTime);
            }
            catch (BadRequestException)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Data od nie może być datą wcześniejszą niż dzisiejsza oraz musi być wcześniejsza niż data do."
                }, "List");
            }

            roomListViewModel.Rooms = rooms;
            roomListViewModel.CurrentLocation = currentLocation;

            return View(new RoomListViewModel
            {
                Rooms = rooms,
                CurrentLocation = currentLocation,
                StartTime = startTime,
                EndTime = endTime
            });
        }

        public async Task<IActionResult> Details(int id)
        {
            Room room;
            
            try
            {
                room = await _offerService.GetRoom(id);
            }
            catch (NotFoundException)
            {
                return SetAlertCookieAndRedirect(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrany pokój nie istnieje."
                }, "List");
            }
            
            return View(room);
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
