namespace TwigaCRM.Models
{
    public class AppRole: TimeStamps
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            //Navigation property
            public List<AppUser> Users { get; set; }


            public List<AppRole_Permission> AppRolePermissions { get; set; }
        //public ICollection<AppRole_Permission> Permissions { get; set; } = new HashSet<AppRole_Permission>();
    }
}
