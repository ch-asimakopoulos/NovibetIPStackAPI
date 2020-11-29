using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.IPRelated;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovibetIPStackAPI.Infrastructure.Persistence.Caching
{
    /// <summary>
    /// An implementation of the IIPDetailsRepository. This class implements a decorator pattern to allow us to retrieve IP detail items from an in-memory cache, a persistence repository or the IP Stack API.
    /// </summary>
    public class CachedIPDetailsRepositoryDecorator : IIPDetailsRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IIPInfoProvider _IIPInfoProvider;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        private readonly SemaphoreSlim _cacheLock;
        private readonly IPDetailsRepository _repository;
        private readonly ILogger<CachedIPDetailsRepositoryDecorator> _logger;
        public CachedIPDetailsRepositoryDecorator(AppDbContext appDbContext, IMemoryCache cache, IIPInfoProvider ipInfoProvider, IConfiguration configuration, ILogger<CachedIPDetailsRepositoryDecorator> logger)
        {
            _repository = new IPDetailsRepository(appDbContext);
            _cache = cache;
            _IIPInfoProvider = ipInfoProvider;
            _logger = logger;
            _cacheLock = new SemaphoreSlim(1);


            int AbsoluteExpirationTimeInSeconds;
            if (!int.TryParse(configuration["MemoryCacheOptions:AbsoluteExpirationTimeInSeconds"], out AbsoluteExpirationTimeInSeconds))
            {
                _logger.LogError($"Could not find the absolute expiration time in seconds for the in memory cache in the provided configuration.");
                throw new Exception($"Could not find the absolute expiration time in seconds for the in memory cache in the provided configuration.");
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
            //to do: _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
            return result;
        }
        public async Task<IPDetailsModel> GetByIdAsync(long id)
        {
            IPDetailsModel result = await _repository.GetByIdAsync(id);
            //to do: _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
            return result;
        }
        public IPDetailsModel GetByIPAddress(string ip)
        {
            IPDetailsModel result;
            try
            {
                _cacheLock.Wait();
                if (!_cache.TryGetValue(key: ip, out result))
                {
                    _logger.LogDebug($"Did not find ip: {ip} in cache.");
                    result = _repository.GetByIPAddress(ip);
                    if (result != null)
                    {
                        _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
                        _logger.LogDebug($"Found ip: {ip} in database");
                        return result;
                    }

                    _logger.LogDebug($"Did not find ip: {ip} in database. Will try to get it via the IPStack API.");
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get ip: {ip} in any of the three ways possible (IPStack API, Caching or Repository). Exception message: {ex.Message}");
                throw ex;
            }
            finally
            {
                _cacheLock.Release();
            }

            _logger.LogDebug($"Found ip: {ip} in memory cache.");
            return result;
        }
        public async Task<IPDetailsModel> GetByIPAddressAsync(string ip)
        {
            IPDetailsModel result;
            try
            {
                _cacheLock.Wait();
                if (!_cache.TryGetValue(key: ip, out result))
                {
                    result = _repository.GetByIPAddress(ip);
                    if (result != null)
                    {
                        _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
                        return result;
                    }

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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get ip: {ip} in any of the three ways possible (IPStack API, Caching or Repository). Exception message: {ex.Message}");
                throw ex;
            }
            finally
            {
                _cacheLock.Release();
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
