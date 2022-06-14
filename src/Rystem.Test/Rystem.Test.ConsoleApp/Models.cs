namespace Rystem.Test.ConsoleApp
{
    public class User
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public string? Password { get; set; }
        public string[]? Claims { get; set; }
        public Dictionary<string, string>? Values { get; set; }
        public IEnumerable<string>? Cursors { get; set; }
    }
}
