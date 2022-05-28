using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotelix.Client.Models.Api;
using Hotelix.Client.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hotelix.Client.Models
{
    public class OrderForm
    {
        private static Room Room { get; set; }
        private static DateTime StartTime { get; set; }
        private static DateTime EndTime { get; set; }

        private OrderForm(/*, Room room, DateTime startTime, DateTime endTime*/)
        {
            /*Room = room;
            StartTime = startTime;
            EndTime = endTime;*/
        }

        public static OrderForm InitOrderForm(IServiceProvider services)
        {
            /*var room = Room;
            var startTime = StartTime;
            var endTime = EndTime;*/
            
            return new OrderForm(/*, room, startTime, endTime*/);
        }

        public Room GetRoom()
        {
            return Room;
        }

        public DateTime GetStartTime()
        {
            return StartTime;
        }

        public DateTime GetEndTime()
        {
            return EndTime;
        }

        public void SetOrderForm(Room room, DateTime startTime, DateTime endTime)
        {
            Room = room;
            StartTime = startTime;
            EndTime = endTime;
        }

        public void ClearOrder()
        {
            Room = null;
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
        }

        public decimal GetTotalPrice()
        {
            return IsValid() ? Room.PricePerNight * Convert.ToDecimal((EndTime - StartTime).TotalDays) : 0;
        }

        public bool IsValid()
        {
            return Room != null && StartTime != DateTime.MinValue && EndTime != DateTime.MinValue;
        }
        
        
        
        /*private readonly ISession _session;
        public string OrderFormId { get; set; }
        private static int RoomIdKey { get; set; }
        private string StartTimeKey = "OrderForm.StartTime";
        private string EndTimeKey = "OrderForm.EndTime";

        private OrderForm(ISession session, int roomIdKey)
        {
            _session = session;
            RoomIdKey = roomIdKey;
        }

        public static OrderForm GetOrderForm(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            
            *//*session.SetInt32("OrderForm.RoomId", 0);
            session.SetString("OrderForm.StartTime", null);
            session.SetString("OrderForm.EndTime", null);*//*

            var tmpRoomIdKey = RoomIdKey == 0 ? 22 : RoomIdKey;

            *//*if (session.GetInt32(RoomIdKey) == null)
            {
                session.SetInt32("OrderForm.RoomId", 34);
            }*//*
            session.SetString("OrderForm.StartTime", "2021-12-21");
            session.SetString("OrderForm.EndTime", "2021-12-29");

            //return new OrderForm(session) {RoomIdKey = "OrderForm.RoomId", StartTimeKey = "OrderForm.StartTime", EndTimeKey = "OrderForm.EndTime"};
            return new OrderForm(session, tmpRoomIdKey) {StartTimeKey = "OrderForm.StartTime", EndTimeKey = "OrderForm.EndTime"};
        }

        public void SetRoomId(int id)
        {
            //_session.SetInt32("OrderForm.RoomId", id);
            RoomIdKey = id;
        }
        
        public int GetRoomId()
        {
            //return _session.GetInt32("OrderForm.RoomId") ?? 0;
            return RoomIdKey;
        }
        
        public string GetStartTime()
        {
            return _session.GetString("OrderForm.StartTime");
        }
        
        public string GetEndTime()
        {
            return _session.GetString("OrderForm.EndTime");
        }*/
    }
}
