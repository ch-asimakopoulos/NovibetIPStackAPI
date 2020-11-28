namespace NovibetIPStackAPI.IPStackWrapper.Models
{
    /// <summary>
    /// The error information IPStack's API returns when the request is not successful.
    /// </summary>
    public class IPStackError
    {
        public int? code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }
}
