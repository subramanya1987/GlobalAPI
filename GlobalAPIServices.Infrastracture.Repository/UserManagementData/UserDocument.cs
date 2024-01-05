using System;
using System.Collections.Generic;

namespace GlobalAPIServices.Infrastracture.Repository.UserManagementData
{
    public partial class UserDocument
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string Filename { get; set; } = null!;
        public string ServerFileName { get; set; } = null!;
        public string ServerPath { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual User Id1 { get; set; } = null!;
        public virtual Application IdNavigation { get; set; } = null!;
    }
}
