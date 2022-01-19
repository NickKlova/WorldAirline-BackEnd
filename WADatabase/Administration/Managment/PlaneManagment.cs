using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WADatabase.Administration.Clients;
using WADatabase.Models.API.Request;
using WADatabase.Models.API.Response;

namespace WADatabase.Administration.Managment
{
    public class PlaneManagment  : Interfaces.IPlane
    {
        private readonly WorldAirlinesClient _db;
        public PlaneManagment(WorldAirlinesClient dbClient)
        {
            _db = dbClient;
        }
        public async Task<IEnumerable<ReturnPlane>> GetAllPlanesAsync()
        {
            await using (_db)
            {
                var planes = _db.context.Planes
                    .ToListAsync()
                    .Result;

                if (planes == null)
                    return null;

                List<ReturnPlane> response = new List<ReturnPlane>();

                foreach (var plane in planes)
                {
                    ReturnPlane item = new ReturnPlane
                    {
                        Id = plane.Id,
                        Number = plane.Number,
                        Model = plane.Model,
                        ManufactureDate = plane.ManufactureDate.ToShortDateString(),
                        LifeTime = plane.LifeTime,
                        Ok = plane.Ok
                    };

                    response.Add(item);
                }

                return response;
            }
        }
        public async Task<ReturnPlane> GetPlaneAsync(int? id)
        {
            if (id == null)
                return null;

            await using (_db)
            {
                var plane = _db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (plane == null)
                    return null;

                ReturnPlane response = new ReturnPlane
                {
                    Id = plane.Id,
                    Number = plane.Number,
                    Model = plane.Model,
                    ManufactureDate = plane.ManufactureDate.ToShortDateString(),
                    LifeTime = plane.LifeTime,
                    Ok = plane.Ok
                };

                return response;
            }
        }
        public async Task CreatePlaneAsync(ReceivedPlane incomingData)
        {
            await using (_db)
            {
                Models.DB_Request.Plane plane = new Models.DB_Request.Plane
                {
                    Model = incomingData.Model,
                    Number = incomingData.Number,
                    ManufactureDate = incomingData.ManufactureDate,
                    Ok = incomingData.Ok,
                    LifeTime = incomingData.LifeTime
                };

                _db.context.Add(plane);
                _db.context.SaveChanges();
            }
        }
        public async Task DeletePlaneAsync(int id)
        {
            await using (_db)
            {
                var plane = _db.context.Planes
                    .ToListAsync()
                    .Result
                    .FirstOrDefault(x => x.Id == id);

                if (plane == null)
                    throw new Exception("The specified aircraft was not found in the database!");

                _db.context.Remove(plane);
                _db.context.SaveChanges();
            }
        }
    }
}
