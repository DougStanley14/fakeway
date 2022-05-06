
using microsvc_authr.IdentModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.IdentModel
{
    [Table(nameof(UserRole))]
    public class UserRole
    {
        [ForeignKey(nameof(microsvc_authr.IdentModel.User.Id))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(microsvc_authr.IdentModel.Role.Id))]
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}
