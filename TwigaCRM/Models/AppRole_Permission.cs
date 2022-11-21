namespace TwigaCRM.Models
{
    public class AppRole_Permission : TimeStamps
    {
        public int Id { get; set; }


        public int AppRoleId{ get; set; }
        public AppRole AppRole { get; set; }


        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
