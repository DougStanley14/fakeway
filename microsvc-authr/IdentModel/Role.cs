using Microsoft.AspNetCore.Identity;
using microsvc_authr.IdentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.IdentModel
{
    [Table(nameof(Role))]
    public class Role : IdentityRole<long>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
