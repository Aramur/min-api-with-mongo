using MongoDB.Driver;
using MongoDB.Bson;
using DotNetEnv;

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
    Environment.Exit(0);
}
var client = new MongoClient(connectionString);
var databaseNames = client.ListDatabaseNames().ToList();
Console.WriteLine("Vorhandene DBs: " + string.Join(",", databaseNames));

var datbase = client.GetDatabase("M165");
var collections = datbase.ListCollectionNames().ToList();
Console.WriteLine("Vorhandene Collections: " + string.Join(",", collections));

var collection = datbase.GetCollection<BsonDocument>("Movies");

var filter = Builders<BsonDocument>.Filter.Eq("Year", 2012);
var twentytwelf = collection.Find(filter).FirstOrDefault();
Console.WriteLine(twentytwelf["Title"]);

