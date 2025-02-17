namespace FabricWebApi
{
    public class FabricRequestLog
    {
        public DateTime CreatedAt { get; init; }

        public string CreatedBy { get; init; }

        public string Request { get; init; }

        public FabricRequestLog(string createdBy, string request)
        {
            CreatedAt = DateTime.Now;
            CreatedBy = createdBy;
            Request = request;
        }
    }
}
