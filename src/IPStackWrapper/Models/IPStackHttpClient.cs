using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NovibetIPStackAPI.IPStackWrapper.Models
{
    /// <summary>
    /// Creates a configured instance of an HttpClient, ready to consume IPStack's API. 
    /// </summary>
    public class IPStackHttpClient : HttpClient
    {
        public readonly string APIKey;

        public IPStackHttpClient(IConfiguration configuration) : base()
        {
            string BaseURL = configuration["IPStackAPI:BaseURL"];
            APIKey = configuration["IPStackAPI:APIKey"];

            //default HttpClient timeout is 100 secs. Could change it here, if needed.
            this.BaseAddress = new Uri(BaseURL);

        }
    }
}
