using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;

namespace WADatabase.Administration.Managment
{
    public class PlaneManagment
    {
        public async Task<IEnumerable<Models.API.Response.ReturnPlane>> GetAllPlanes()
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var planes = db.context.Planes
                    .ToListAsync()
                    .Result;


                List<Models.API.Response.ReturnPlane> response = new List<Models.API.Response.ReturnPlane>();

                foreach (var plane in planes)
                {
                    Models.API.Response.ReturnPlane item = new Models.API.Response.ReturnPlane
                    {
                        Id = plane.Id,
                        Number = plane.Number,
                        Model = plane.Model,
                        ManufactureDate = plane.ManufactureDate,
                        LifeTime = plane.LifeTime,
                        Ok = plane.Ok
                    };

                    response.Add(item);
                }

                return response;
            }
        }

        public async Task<Models.API.Response.ReturnPlane> GetPlaneById(int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var plane = db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                Models.API.Response.ReturnPlane item = new Models.API.Response.ReturnPlane
                {
                    Id = plane.Id,
                    Number = plane.Number,
                    Model = plane.Model,
                    ManufactureDate = plane.ManufactureDate,
                    LifeTime = plane.LifeTime,
                    Ok = plane.Ok
                };

                return item;
            }
        }

        public async Task CreatePlane(ReceivedPlane incomingData)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                Models.DB_Request.Plane plane = new Models.DB_Request.Plane
                {
                    Model = incomingData.Model,
                    Number = incomingData.Number,
                    ManufactureDate = incomingData.ManufactureDate,
                    Ok = incomingData.Ok,
                    LifeTime = incomingData.LifeTime
                };

                db.context.Add(plane);
                db.context.SaveChanges();
            }
        }

        public async Task DeletePlane (int id)
        {
            WorldAirlinesClient db = new WorldAirlinesClient();

            await using (db.context)
            {
                var plane = db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                db.context.Remove(plane);
                db.context.SaveChanges();
            }
        }
    }
}
