using System;
using System.Collections.Generic;

namespace GlobalAPIServices.Infrastracture.Repository.UserManagementData
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string UserName { get; set; } = null!;
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public int? UsernameChangeLimit { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicturePath { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Application Application { get; set; } = null!;
        public virtual UserDocument? UserDocument { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
