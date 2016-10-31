using System.Collections.Generic;

namespace LightBlue.Database
{
    public class WebRole : Role
    {
        public virtual List<Site> Sites { get; set; }

        public WebRole()
        {
            Sites = new List<Site>();
        }
    }
}