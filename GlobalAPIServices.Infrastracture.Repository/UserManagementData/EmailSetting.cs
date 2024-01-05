using System;
using System.Collections.Generic;

namespace GlobalAPIServices.Infrastracture.Repository.UserManagementData
{
    public partial class EmailSetting
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string EmailId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SmtpAddress { get; set; } = null!;
        public int PortNumber { get; set; }
        public bool EnableSsl { get; set; }
        public bool IsActive { get; set; }

        public virtual Application Application { get; set; } = null!;
    }
}
