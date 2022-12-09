using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TimeT.Data.Dtos.Service;
using TimeT.Data.Entities;
using TimeT.Helpers;

namespace TimeT.Data.Repository
{
    public interface IServicesRepository
    {
        Task Create(Service service);
        Task Delete(Service service);
        Task<Service> Get(int id);
        Task<IEnumerable<Service>> GetAll();
        Task<PagedList<Service>> GetAll(ServiceSearchParam serviceSearchParam);
        Task Update(Service service);
    }

    public class ServicesRepository : IServicesRepository
    {
        private readonly RestContext _restContext;

        public ServicesRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            return await _restContext.Services.ToListAsync();
        }
        public async Task<PagedList<Service>> GetAll(ServiceSearchParam serviceSearchParam)
        {
            var queryable = _restContext.Services.AsQueryable().OrderBy(o => o.name);
            return await PagedList<Service>.CreateAsync(queryable, serviceSearchParam.pageNumber, serviceSearchParam.pageSize);
        }
        public async Task<Service> Get(int id)
        {
            return await _restContext.Services.FirstOrDefaultAsync(o => o.id == id);
        }
        public async Task Create(Service service)
        {
            _restContext.Services.Add(service);
            await _restContext.SaveChangesAsync();
        }

        public async Task Update(Service service)
        {
            _restContext.Services.Update(service);
            await _restContext.SaveChangesAsync();
        }

        public async Task Delete(Service service)
        {
            _restContext.Services.Remove(service);
            await _restContext.SaveChangesAsync();
        }

    }
}
