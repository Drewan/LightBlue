namespace LightBlue.Database
{
    public class Binding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EndPointName { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
    }
}