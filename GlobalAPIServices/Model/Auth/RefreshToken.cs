namespace GlobalAPIServices.Model.Auth
{
    public class RefreshToken
    {
        public string RefToken { get; set; } = string.Empty;
        public DateTime TockenCreatedDateTime { get; set; }
        public DateTime TockenExpiredDateTime { get; set; }
    }
}
