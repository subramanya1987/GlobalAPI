using System;
using System.Collections.Generic;

namespace GlobalAPIServices.Infrastracture.Repository.UserManagementData
{
    public partial class Application
    {
        public Application()
        {
            EmailSettings = new HashSet<EmailSetting>();
            Roles = new HashSet<Role>();
            UserRoles = new HashSet<UserRole>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PinCode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Mobile1 { get; set; }
        public string? Mobile2 { get; set; }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual UserDocument? UserDocument { get; set; }
        public virtual ICollection<EmailSetting> EmailSettings { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
