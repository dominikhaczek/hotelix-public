using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Exceptions;
using Hotelix.Client.Models;
using Hotelix.Client.Models.Api;
using Hotelix.Client.Models.Database;
using Hotelix.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotelix.Client.Controllers
{
    [Authorize(Roles = "Administrator,Pracownik")]
    public class OfferManagementController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferManagementController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _offerService.GetAllRooms();
            rooms = rooms.Concat(await _offerService.GetDeletedRooms());

            var offerManagementModel = new OfferManagementModel(rooms);

            return View(offerManagementModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoom()
        {
            var locationsList = (await _offerService.GetAllLocations()).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            })
                .ToList();

            ViewBag.LocationsList = locationsList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(AddEditRoomModel addEditCarModel)
        {
            if (ModelState.IsValid)
            {
                await _offerService.AddRoom(addEditCarModel.Room);
                SetAlertCookie(new AlertModel
                {
                    Type = "success",
                    Message = "Pomyślnie dodano pokój."
                });
            }
            else
            {
                SetAlertCookie(new AlertModel
                {
                    Type = "danger",
                    Message = "Pokój nie został dodany - nie wszystkie wymagane pola zostały poprawnie uzupełnione."
                });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int roomId)
        {
            var locationsList = (await _offerService.GetAllLocations()).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            })
                .ToList();

            ViewBag.LocationsList = locationsList;

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
            
            return View(new AddEditRoomModel
            {
                Room = room
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRoom(AddEditRoomModel addEditCarModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _offerService.UpdateRoom(addEditCarModel.Room);
                    SetAlertCookie(new AlertModel
                    {
                        Type = "success",
                        Message = "Pomyślnie zedytowano pokój."
                    });
                }
                catch (NotFoundException)
                {
                    SetAlertCookie(new AlertModel
                    {
                        Type = "danger",
                        Message = "Wybrany pokój nie istnieje."
                    });
                }
            }
            else
            {
                SetAlertCookie(new AlertModel
                {
                    Type = "danger",
                    Message = "Pokój nie został zedytowany - nie wszystkie wymagane pola zostały poprawnie uzupełnione."
                });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveRoom(int roomId)
        {
            try
            {
                await _offerService.HideRoom(roomId);
                SetAlertCookie(new AlertModel
                {
                    Type = "success",
                    Message = "Pomyślnie usunięto pokój."
                });
            }
            catch (NotFoundException)
            {
                SetAlertCookie(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrany pokój nie istnieje."
                });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BringBackRoom(int roomId)
        {
            try
            {
                await _offerService.UnHideRoom(roomId);
                SetAlertCookie(new AlertModel
                {
                    Type = "success",
                    Message = "Pomyślnie przywrócono pokój."
                });
            }
            catch (NotFoundException)
            {
                SetAlertCookie(new AlertModel
                {
                    Type = "danger",
                    Message = "Wybrany pokój nie istnieje."
                });
            }

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
