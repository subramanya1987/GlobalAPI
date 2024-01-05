using System;
using System.Collections.Generic;

namespace GlobalAPIServices.Infrastracture.Repository.UserManagementData
{
    public partial class UserRole
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Application Application { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
