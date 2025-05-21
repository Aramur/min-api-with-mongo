using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class MongoMovieService : IMovieService
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoCollection<Movie> _movieCollection;

    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        _mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = _mongoClient.GetDatabase(options.Value.DatabaseName);
        _movieCollection = database.GetCollection<Movie>(options.Value.CollectionName);
    }
    public string Check()
    {
        try
        {
            var databaseNames = _mongoClient.ListDatabaseNames().ToList();

            return "Zugriff auf MongoDB ok. Vorhandene DBs: " + string.Join(",", databaseNames);
        }
        catch (System.Exception e)
        {
            return "Zugriff auf MongoDB funktioniert nicht: " + e.Message;
        }
    }
    public void Create(Movie movie)
    {
        _movieCollection.InsertOne(movie);
    }
    public IEnumerable<Movie> Get()
    {
        return _movieCollection.Find(_ => true).ToList();
    }
    public Movie Get(string id)
    {
        return _movieCollection.Find(movie => movie.Id == id).FirstOrDefault();
    }
    public void Update(string id, Movie movie)
    {
        _movieCollection.ReplaceOne(oldMovie => oldMovie.Id == id, movie);
    }
    public void Remove(string id)
    {
        _movieCollection.DeleteOne(movie => movie.Id == id);
    }
}