namespace LightBlue.Database
{
    public class ConfigurationSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int CloudRoleId { get; set; }
        public virtual CloudRole CloudRole { get; set; }
    }
}