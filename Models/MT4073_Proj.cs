using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GetECINo.Models
{
    public class MT4073_Proj
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public object ecino { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
    }
}