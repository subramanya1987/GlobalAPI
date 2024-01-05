namespace GlobalAPIServices.Model.Auth
{
    public class User
    { 
        public string Username { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TockenCreatedDateTime { get; set; }
        public DateTime TockenExpiredDateTime { get; set; }
    }
}
