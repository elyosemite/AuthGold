using AuthGold.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthGold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LogController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Logging> Get()
        {
            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            var database = client.GetDatabase("LoggingEinblick");
            var collection = database.GetCollection<Logging>("logging");


            // Add

            collection.InsertOne(new Logging
            {
                Identifier = ObjectId.GenerateNewId(),
                Name = "Yuri Melo",
                Time = DateTime.Now
            });

            return collection.Find(o => o.Name != null).ToList();
        }
    }
}
