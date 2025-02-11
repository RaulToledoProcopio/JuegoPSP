using System;
using DotNetEnv;
using MongoDB.Driver;

class Program
{
    static void Main()
    {
        Env.Load();
        string connectionString = Env.GetString("URL_MONGODB");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("Api_Godot");
        var collection = database.GetCollection<dynamic>("CollCrono");

        Console.WriteLine("Conexión a MongoDB establecida correctamente.");
    }
}