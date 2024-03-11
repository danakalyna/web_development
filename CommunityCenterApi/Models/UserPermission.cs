using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CommunityCenterApi.Models
{
    public class UserPermission
    {
        public int UserPermissionId { get; set; }
        public string PermissionName { get; set; }
        // Other properties for permissions as needed

        // Navigation property to User
        public Guid UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
