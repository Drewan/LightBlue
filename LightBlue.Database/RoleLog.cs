namespace LightBlue.Database
{
    public class RoleLog
    {
        public int RoleLogId { get; set; }
        
        public string Message { get; set; }

        public int CloudRoleId { get; set; }

        public virtual CloudRole CloudRole { get; set; }
    }
}