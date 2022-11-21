namespace TwigaCRM.Models
{
    public class Permission : TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        //Navigation properties

        public List<AppRole_Permission> AppRolePermissions { get; set; }
        //public ICollection<AppRole_Permission> AppRoles { get; set; } = new HashSet<AppRole_Permission>();

    }
}
