namespace LightBlue.Database
{
    public class LocalResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public int CloudRoleId { get; set; }
        public virtual WebRole WebRole { get; set; }
    }
}