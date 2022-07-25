// See https://aka.ms/new-console-template for more information
using Rystem.Test.ConsoleApp;
using System.Reflection;
using System.Text.Csv;

Console.WriteLine("Hello, World!");

#pragma warning disable S125 // Sections of code should not be commented out
                            //var user2 = Mocking.CreateInstance<IUser>();
                            //user2.Name = "falnde";
                            //var user3 = Mocking.CreateInstance<AUser>("a", "b", "c", "d", "e", "f", "g", "h", "s");
                            //var user4 = Mocking.CreateInstance<AUser>("a", "b", "c", "d", "e", "f", "g", "h");
                            //var user5 = Mocking.CreateInstance<AUser>("a", "b", "c", "d", "e", "f", "g");
                            //user3.Name = "dadsad";
                            ////user3.Axa = "dasdsa";
                            //user3.Alof = "eeeee";
                            //var mock = new Mock<IUser>();
                            //foreach (var property in typeof(IUser).FetchProperties())
                            //{
                            //    //mock.Setup(x => x.Name).Returns(property.Name);
                            //    var method = typeof(DynamicExpressionParser).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            //        .Where(x => x.Name == "ParseLambda" && x.ContainsGenericParameters).Skip(4).First()
                            //        .MakeGenericMethod(typeof(IUser), property.PropertyType);
                            //    var expression = method.Invoke(null, new object[4] { ParsingConfig.Default, false, $"x => x.{property.Name}", Array.Empty<object>() });
                            //    var aa = mock.GetType().GetMethods().Last(x => x.Name == "Setup")
                            //        .MakeGenericMethod(typeof(string))
                            //        .Invoke(mock, new object[1] { expression });
                            //    var qq = aa.GetType().GetMethods().First(x => x.Name == "Returns" && !x.ContainsGenericParameters);
                            //    qq.Invoke(aa, new object[1] { "Hello" });
                            //}
                            //var myUser = mock.Object;
                            //myUser.Name = "Test";

List<User> users = new();
#pragma warning restore S125 // Sections of code should not be commented out
User user = new()
{
    Claims = new[] { "a", "b", "c" },
    Id = 1,
    Name = "dasdsa",
    Password = "dasdas",
    Values = new Dictionary<string, string> { { "a", "b" }, { "b", "b" }, { "c", "b" } },
    Cursors = new Stack<string>(),
    Pages = new List<string>() { "a", "b", "c" },
    Queues = new List<string>() { "a", "b", "c" }
};
(user.Cursors as Stack<string>)!.Push("a");
(user.Cursors as Stack<string>)!.Push("b");
(user.Cursors as Stack<string>)!.Push("c");
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
var x = users.ToCsv('&');

var t = x.FromCsv<List<User>>('&');
Console.WriteLine(t.Count);