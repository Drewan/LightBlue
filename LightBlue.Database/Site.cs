using System.Collections.Generic;

namespace LightBlue.Database
{
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Binding> Bindings { get; set; }

        public int WebRoleId { get; set; }
        public virtual WebRole WebRole { get; set; }
    }
}