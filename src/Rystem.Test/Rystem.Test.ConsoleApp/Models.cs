using System.Text.Csv;
using System.Timers;

namespace Rystem.Test.ConsoleApp
{
    public class User : IUser
    {
        [CsvProperty(1)]
        public string? Name { get; set; }
        [CsvProperty(0)]
        public int Id { get; set; }
        [CsvProperty(4)]
        public string? Password { get; set; }
        [CsvProperty(3)]
        public string[]? Claims { get; set; }
        [CsvProperty(2)]
        public Dictionary<string, string>? Values { get; set; }
        public IEnumerable<string>? Cursors { get; set; }
        [CsvProperty(10)]
        public ICollection<string>? Pages { get; set; }
        [CsvProperty(-5)]
        public IList<string>? Queues { get; set; }
    }
    public interface IUser
    {
        string? Name { get; set; }
        int Id { get; set; }
        string? Password { get; set; }
    }
    public abstract class AUser
    {
        public abstract string Name { get; set; }
        public abstract string Id { get; set; }
        public string Alof { get; set; }
        public string Axa { get; }
        public string D { get; }
        public string E { get; }
        public string F { get; }
        public string G { get; }
        public string H { get; }
        public string S { get; }
        public AUser(string a, string b, string c, string d, string e, string f, string g)
        {
            Name = a;
            Id = b;
            Axa = c;
            D = d;
            E = e;
            F = f;
            G = g;
        }
        public AUser(string a, string b, string c, string d, string e, string f, string g, string h)
        {
            Name = a;
            Id = b;
            Axa = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
        }
        public AUser(string a, string b, string c, string d, string e, string f, string g, string h, string s)
        {
            Name = a;
            Id = b;
            Axa = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
            S = s;
        }
    }
    public class Something : IBackgroundJob
    {
        public Task ActionToDoAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnException(Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
