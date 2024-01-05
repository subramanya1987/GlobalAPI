using Newtonsoft.Json;

namespace GlobalAPIServices.Model
{
    public class ServiceResponse
    {
        public ServiceResponse(int code,string message,string data) 
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public int Code {  get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
