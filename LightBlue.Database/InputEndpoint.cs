namespace LightBlue.Database
{
    public class InputEndpoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }

        public int CloudRoleId { get; set; }
        public virtual WebRole WebRole { get; set; }
    }
}