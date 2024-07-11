using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MarketApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("pricebefore")]
        public decimal PriceBefore { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("imageurl")]
        public string ImageUrl { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("market")]
        public string Market { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("marketlogo")]
        public string MarketLogo { get; set; }
    }
}
