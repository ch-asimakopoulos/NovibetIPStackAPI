using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NovibetIPStackAPI.Core.Interfaces;
using NovibetIPStackAPI.Core.Models;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Persistence
{
    /// <summary>
    /// This class implements a decorator pattern to allow us to retrieve items from an in-memory cache, a persistence repository or the IP Stack API.
    /// </summary>
    public class CachedIPDetailsRepositoryDecorator : IIPDetailsRepository
    {
        private readonly IIPDetailsRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IIPInfoProvider _IIPInfoProvider;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        public CachedIPDetailsRepositoryDecorator(IIPDetailsRepository repository, IMemoryCache cache, IIPInfoProvider ipInfoProvider, IConfiguration configuration)
        {
            _repository = repository;
            _cache = cache;
            _IIPInfoProvider = ipInfoProvider;

            int AbsoluteExpirationTimeInSeconds;
            if (!int.TryParse(configuration["MemoryCacheOptions:AbsoluteExpirationTimeInSeconds"], out AbsoluteExpirationTimeInSeconds))
            {
                //TO FIX
                throw new Exception();
            }

            _memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds((double)AbsoluteExpirationTimeInSeconds));
        }

        public async Task<IPDetailsModel> AddAsync(IPDetailsModel entity)
        {
            IPDetailsModel result = await _repository.AddAsync(entity);
            _cache.Set(key: entity.IP, value: result, options: _memoryCacheEntryOptions);
            return result;
        }
        public async Task DeleteAsync(IPDetailsModel entity)
        {
            _cache.Remove(key: entity.IP);
            await _repository.DeleteAsync(entity);
        }
        public async Task DeleteByIdAsync(long id)
        {
            await _repository.DeleteByIdAsync(id);
        }
        public IPDetailsModel GetById(long id)
        {
            IPDetailsModel result = _repository.GetById(id);
            //_cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
            return result;
        }
        public async Task<IPDetailsModel> GetByIdAsync(long id)
        {
            IPDetailsModel result = await _repository.GetByIdAsync(id);
            //_cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
            return result;
        }
        public IPDetailsModel GetByIPAddress(string ip)
        {
            IPDetailsModel result;
            try
            {
                if (!_cache.TryGetValue(key: ip, out result))
                {
                    result = _repository.GetByIPAddress(ip);
                    _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
                    return result;
                }
            }
            catch
            {
                IPDetails dets = _IIPInfoProvider.GetDetails(ip);
                result = new IPDetailsModel()
                {
                    City = dets.City,
                    Continent = dets.Continent,
                    Country = dets.Country,
                    IP = ip,
                    Latitude = dets.Latitude,
                    Longitude = dets.Longitude,
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow
                };

                Task<IPDetailsModel> ipDetailsModelAddTask = new Task<IPDetailsModel>(() =>
                    {
                        return AddAsync(result).Result;
                    });

                ipDetailsModelAddTask.RunSynchronously();

                return ipDetailsModelAddTask.Result;
            }

            return result;
        }
        public async Task<IPDetailsModel> GetByIPAddressAsync(string ip)
        {
            IPDetailsModel result;
            try
            {
                if (!_cache.TryGetValue(key: ip, out result))
                {
                    result = _repository.GetByIPAddress(ip);
                    _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
                    return result;
                }
            }
            catch
            {
                //could also go for the IPInfoProvider class and run the async method here, or break the pdf's contract
                //and add a GetDetailsAsync method to the contract.
                IPDetails detsFromIpInfoProvider = _IIPInfoProvider.GetDetails(ip); 

                result = new IPDetailsModel()
                {
                    City = detsFromIpInfoProvider.City,
                    Continent = detsFromIpInfoProvider.Continent,
                    Country = detsFromIpInfoProvider.Country,
                    IP = ip,
                    Latitude = detsFromIpInfoProvider.Latitude,
                    Longitude = detsFromIpInfoProvider.Longitude,
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow
                };

                return await AddAsync(result);
                
            }

            return result;
        }
        public List<IPDetailsModel> List()
        {
            return _repository.List();
        }
        public async Task<List<IPDetailsModel>> ListAsync()
        {
            return await _repository.ListAsync();
        }
        public async Task UpdateAsync(IPDetailsModel entity)
        {
            await _repository.UpdateAsync(entity);
            _cache.Set(key: entity.IP, value: entity, options: _memoryCacheEntryOptions);
        }

    }
}
