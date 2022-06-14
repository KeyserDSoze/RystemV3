// See https://aka.ms/new-console-template for more information
using Rystem.Test.ConsoleApp;
using System.Text.Csv;

Console.WriteLine("Hello, World!");

List<User> users = new();
User user = new()
{
    Claims = new[] { "a", "b", "c" },
    Id = 1,
    Name = "dasdsa",
    Password = "dasdas",
    Values = new Dictionary<string, string> { { "a", "b" }, { "b", "b" }, { "c", "b" } },
    Cursors = new List<string> { "a", "b", "c" }
};
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
users.Add(user);
var x = users.ToCsv();
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0059 // Unnecessary assignment of a value
var t = x.FromCsv<List<User>>();
#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0219 // Variable is assigned but its value is never used
string apollo = "";
#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning restore CS0219 // Variable is assigned but its value is never used
