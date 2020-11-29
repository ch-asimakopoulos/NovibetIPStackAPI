using NovibetIPStackAPI.IPStackWrapper.Exceptions;
using NovibetIPStackAPI.IPStackWrapper.Models;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using Microsoft.Extensions.Logging;

namespace NovibetIPStackAPI.IPStackWrapper.Services
{
    /// <summary>
    /// Implements the <see cref="IIPInfoProvider"/> interface, while adding the ability to get the geolocation details asynchronously.
    /// </summary>
    public class IPInfoProvider : IIPInfoProvider
    {

        private readonly IPStackHttpClient _ipStackHttpClient;
        private readonly ILogger<IPInfoProvider> _logger;
        public IPInfoProvider(IConfiguration configuration, ILogger<IPInfoProvider> logger)
        {
            _ipStackHttpClient = new IPStackHttpClient(configuration);
            _logger = logger;

        }

        /// <summary>
        /// Gets the geolocation details that derive from the specified IP address.
        /// </summary>
        /// <param name="ip">An IP address</param>
        /// <returns>An instance of  an object that implements the <see cref="IPDetails"/> interface.</returns>
        public IPDetails GetDetails(string ip)
        {
            Task<IPDetails> ipDetailsTask;
            try
            {
                ipDetailsTask = new Task<IPDetails>(() =>
                {
                    try
                    {
                        return GetDetailsAsync(ip).Result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });
                ipDetailsTask.RunSynchronously();
                return ipDetailsTask.Result;


            }
            catch (Exception ex)
            {
                throw new IPServiceNotAvailableException(ex.Message, ex);
            }


        }

        /// <summary>
        /// Gets the geolocation details that derive from the specified IP address asynchronously.
        /// </summary>
        /// <param name="ip">An IP address</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains an instance of an object that implements the <see cref="IPDetails"/> interface.</returns>
        public async Task<IPDetails> GetDetailsAsync(string ip)
        {
            IPDetailsDTO detailsDTO = new IPDetailsDTO();

            try
            {
                HttpResponseMessage response = await _ipStackHttpClient.GetAsync($"{ip}?access_key={_ipStackHttpClient.APIKey}");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseJSON = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    IPStackUnsuccessfulResponseInfo unsuccessfulResponseInfo = JsonSerializer.Deserialize<IPStackUnsuccessfulResponseInfo>(responseJSON, options);

                    throw new Exception($"An error occured while getting the IP details for IP: {ip}. Status Code: {response.StatusCode}. Error info: {unsuccessfulResponseInfo.error.info}");
                }

                detailsDTO = JsonSerializer.Deserialize<IPDetailsDTO>(responseJSON, options);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"IPStack API Exception while getting IP: {ip}. Message: {ex.Message}");
                throw new IPServiceNotAvailableException(ex.Message);
            }
            finally
            {
                _ipStackHttpClient.Dispose();
            }

            return detailsDTO;


        }
    }

}
