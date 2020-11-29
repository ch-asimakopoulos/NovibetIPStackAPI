using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
    /// This class implements a decorator pattern to allow us to retrieve items from an in-memory cache, a persistence repository or the IP Stack API.
    /// </summary>
    public class CachedIPDetailsRepositoryDecorator : IIPDetailsRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IIPInfoProvider _IIPInfoProvider;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        private readonly SemaphoreSlim _cacheLock;
        private readonly IPDetailsRepository _repository;

        public CachedIPDetailsRepositoryDecorator(AppDbContext appDbContext, IMemoryCache cache, IIPInfoProvider ipInfoProvider, IConfiguration configuration)
        {
            _repository = new IPDetailsRepository(appDbContext);
            _cache = cache;
            _IIPInfoProvider = ipInfoProvider;
            _cacheLock = new SemaphoreSlim(1);


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
                _cacheLock.Wait();
                if (!_cache.TryGetValue(key: ip, out result))
                {
                    result = _repository.GetByIPAddress(ip);
                    if (result != null)
                    {
                        _cache.Set(key: result.IP, value: result, options: _memoryCacheEntryOptions);
                        return result;
                    }

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
                throw ex;
            }
            finally
            {
                _cacheLock.Release();
            }

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
