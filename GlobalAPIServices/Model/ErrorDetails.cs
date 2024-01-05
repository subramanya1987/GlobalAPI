using Newtonsoft.Json;

namespace GlobalAPIServices.Model
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
