using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);
builder.Services.AddSingleton<IMovieService, MongoMovieService>();

var app = builder.Build();

app.MapGet("/", () => "Minimal API nach Arbeitsauftrag 2");

// docker run --name mongodb -d -p 27017:27017 -v data:/data/db -e MONGO_INITDB_ROOT_USERNAME=gbs -e MONGO_INITDB_ROOT_PASSWORD=geheim mongo
app.MapGet("/check", (IMovieService movieService) =>
{
    return movieService.Check();
});

// Insert Movie
// Wenn das übergebene Objekt eingefügt werden konnte,
// wird es mit Statuscode 200 zurückgegeben.
// Bei Fehler wird Statuscode 409 Conflict zurückgegeben.
app.MapPost("/api/movies", (Movie movie, IMovieService movieService) =>
{
    movieService.Create(movie);
});
// Get all Movies
// Gibt alle vorhandenen Movie-Objekte mit Statuscode 200 OK zurück.
app.MapGet("api/movies", (IMovieService movieService) =>
{
    return movieService.Get();
});
// Get Movie by id
// Gibt das gewünschte Movie-Objekt mit Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapGet("api/movies/{id}", (string id, IMovieService movieService) =>
{
    return movieService.Get(id);
});
// Update Movie
// Gibt das aktualisierte Movie-Objekt zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapPut("/api/movies/{id}", (string id, Movie movie, IMovieService movieService) =>
{
    movieService.Update(id, movie);
});
// Delete Movie
// Gibt bei erfolgreicher Löschung Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapDelete("api/movies/{id}", (string id, IMovieService movieService) =>
{
    movieService.Remove(id);
});

app.Run();
