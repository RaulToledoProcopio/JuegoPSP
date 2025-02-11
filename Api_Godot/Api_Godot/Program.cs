using DotNetEnv;
using MongoDB.Driver;

class Program
{
    static void Main()
    {
        // Cargar el archivo .env
        Env.Load();

        // Obtener la cadena de conexión desde el .env
        string connectionString = Env.GetString("URL_MONGODB");

        // Crear un cliente de MongoDB
        var client = new MongoClient(connectionString);

        // Seleccionar la base de datos (ajústalo con el nombre de tu BD)
        var database = client.GetDatabase("Api_Godot");

        // Obtener una colección (tabla)
        var collection = database.GetCollection<dynamic>("CollCrono");

        Console.WriteLine("Conexión a MongoDB establecida correctamente.");
    }
}