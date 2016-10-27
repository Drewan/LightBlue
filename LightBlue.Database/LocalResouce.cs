namespace LightBlue.Database
{
    public class LocalResouce
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public int CloudRoleId { get; set; }
        public virtual CloudRole CloudRole { get; set; }
    }
}