using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AuthGold.Models
{
    public class Logging
    {
        [BsonId]
        public ObjectId Identifier { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("time")]
        public DateTime Time { get; set; }
    }
}
