using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Xml.Linq;
using TimeT.Data.Dtos.Service;
using TimeT.Data.Dtos.Time;
using TimeT.Data.Entities;
using TimeT.Helpers;

namespace TimeT.Data.Repository
{
    public interface ITimeRepository
    {
        Task Create(Time time);
        Task Delete(Time time);
        Task<Time> Get(int id);
        Task<IEnumerable<Time>> GetAll(int serviceId);
        Task<PagedList<Time>> GetAll(TimeSearchParam timeSearchParam);
        Task Update(Time time);
    }

    public class TimeRepository : ITimeRepository
    {
        private readonly RestContext _restContext;

        public TimeRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<IEnumerable<Time>> GetAll(int serviceId)
        {
            return await _restContext.Times.AsQueryable().Where(x => x.serviceId == serviceId).ToListAsync();
        }
        public async Task<PagedList<Time>> GetAll(TimeSearchParam timeSearchParam)
        {
            var queryable = _restContext.Times.AsQueryable().OrderBy(o => o.name).Where(o => o.serviceId == timeSearchParam.serviceId);
            return await PagedList<Time>.CreateAsync(queryable, timeSearchParam.pageNumber, timeSearchParam.pageSize);
        }
        public async Task<Time> Get(int id)
        {
            return await _restContext.Times.FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task Create(Time time)
        {
            _restContext.Times.Add(time);
            await _restContext.SaveChangesAsync();
        }
        public async Task Update(Time time)
        {
            _restContext.Times.Update(time);
            await _restContext.SaveChangesAsync();
        }
        public async Task Delete(Time time)
        {
            _restContext.Times.Remove(time);
            await _restContext.SaveChangesAsync();
        }

    }
}
