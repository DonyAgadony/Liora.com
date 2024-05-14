using System.Net;
using static Project.Utils;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UUIDNext;
using System.Data.Common;

namespace Project;



class User(string Id, string Username, string Password, int Level)
{
  [Key]
  public string Id { get; set; } = Id;
  public string Username { get; set; } = Username;
  public string Password { get; set; } = Password;
  public int Level { get; set; } = Level;

  public void LevelUp()
  {
    Level++;
  }

}




class Program
{
  public static void Main()
  {
    /*───────────────────────────╮
    │ Creating the server object │
    ╰───────────────────────────*/
    var server = new HttpListener();
    server.Prefixes.Add("http://localhost:5000/");
    server.Start();

    Console.WriteLine("Server started. Listening for requests...");
    Console.WriteLine("Main page on http://localhost:5000/website/html/index.html");

    /*─────────────────────────╮
    │ Processing HTTP requests │
    ╰─────────────────────────*/
    while (true)
    {
      /*─────────────────────────────────────╮
      │ Creating the database context object │
      ╰─────────────────────────────────────*/
      var databaseContext = new DatabaseContext();

      /*────────────────────────────╮
      │ Waiting for an HTTP request │
      ╰────────────────────────────*/
      var serverContext = server.GetContext();
      var response = serverContext.Response;

      try
      {
        /*────────────────────────╮
        │ Handeling file requests │
        ╰────────────────────────*/
        serverContext.ServeFiles();

        /*───────────────────────────╮
        │ Handeling custome requests │
        ╰───────────────────────────*/
        HandleRequests(serverContext, databaseContext);

        /*───────────────────────────────╮
        │ Saving changes to the database │
        ╰───────────────────────────────*/
        databaseContext.SaveChanges();

      }
      catch (Exception e)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e);
        Console.ResetColor();
      }

      /*───────────────────────────────────╮
      │ Sending the response to the client │
      ╰───────────────────────────────────*/
      response.Close();
    }
  }

  public static void HandleRequests(HttpListenerContext serverContext, DatabaseContext databaseContext)
  {
    var request = serverContext.Request;
    var response = serverContext.Response;

    string absPath = request.Url!.AbsolutePath;
    if (absPath == "/addEquations")
    {
      Equations equations = request.GetBody<Equations>();
      Console.WriteLine("Equations Recieved");
      EquationValues RtnToWeb = EquationsProgram.MainEquations(equations);
      string Json = JsonConvert.SerializeObject(RtnToWeb);
      response.ContentType = "application/json";
      response.Write(Json);
      Console.WriteLine($"The solutions are: X = {RtnToWeb.X}, Y = {RtnToWeb.Y}, Z = {RtnToWeb.Z}, W = {RtnToWeb.W}");
    }
    if (absPath == "/getLevel")
    {
      string Id = request.GetBody<string>();
      User user = databaseContext.Users.Find(Id)!;
      response.Write(user.Level);
    }
    if (absPath == "/addLevel")
    {
      string Id = request.GetBody<string>();
      User user = databaseContext.Users.Find(Id)!;
      user.LevelUp();
      response.Write(user.Level);
    }
    if (absPath == "/signUp")
    {
      (string username, string password) = request.GetBody<(string, string)>();

      var userId = Uuid.NewDatabaseFriendly(UUIDNext.Database.SQLite).ToString();

      var user = new User(username, password, userId, 0);
      databaseContext.Users.Add(user);

      response.Write(userId);
    }
    if (absPath == "/logIn")
    {
      (string username, string password) = request.GetBody<(string, string)>();

      User user = databaseContext.Users.First(
        u => u.Username == username && u.Password == password
      )!;
      // here
      response.Write(user.Id);
    }
  }
  public class DatabaseContext : DbContextWrapper
  {
    public DatabaseContext() : base("Database") { }
    public DbSet<User> Users { get; set; }
  }
}