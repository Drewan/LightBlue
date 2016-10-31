using System.Data.Entity;
using System.Data.SqlClient;

namespace LightBlue.Database
{
    public class LightBlueContext : DbContext
    {
        public DbSet<CloudService> CloudServices { get; set; }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<BlobContainer> BlobContainers { get; set; }

        public LightBlueContext() : base(new SqlConnection("Data Source=.;Integrated Security=SSPI;Initial Catalog=LightBlue.Local").ConnectionString)
        {

        }


    }
}
