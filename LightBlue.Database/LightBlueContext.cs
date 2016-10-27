using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBlue.Database
{
    public class LightBlueContext : DbContext
    {
        public DbSet<CloudService> CloudServices { get; set; }

        public DbSet<CloudQueue> Queues { get; set; }
        public DbSet<CloudBlobContainer> BlobContainers { get; set; }

        public LightBlueContext() : base(new SqlConnection("Data Source=.;Integrated Security=SSPI;Initial Catalog=LightBlue.Local").ConnectionString)
        {

        }


    }
}
