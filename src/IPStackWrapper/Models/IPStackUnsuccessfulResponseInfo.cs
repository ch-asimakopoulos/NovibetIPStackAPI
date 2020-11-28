namespace NovibetIPStackAPI.IPStackWrapper.Models
{
    /// <summary>
    /// A POCO object that corresponds to the error JSON object that IPStack returns when the request is unsuccessful.
    /// </summary>
    public class IPStackUnsuccessfulResponseInfo
    {
        public bool success;
        public IPStackError error;

    }
}
