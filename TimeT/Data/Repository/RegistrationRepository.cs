using TimeT.Data.Dtos.Registration;
using TimeT.Data.Entities;
using TimeT.Helpers;
using Microsoft.Win32;
using System.Xml.Linq;
using TimeT.Data.Dtos.Service;
using TimeT.Data.Dtos.Time;
using Microsoft.EntityFrameworkCore;

namespace TimeT.Data.Repository
{
    public interface IRegistrationRepository
    {
        Task Create(Registration registration);
        Task Delete(Registration registration);
        Task<Registration> Get(int id);
        Task<IEnumerable<Registration>> GetAll(int timeId);
        Task<PagedList<Registration>> GetAll(RegistrationSearchPrams registrationSearchPrams);
        Task Update(Registration registration);
    }

    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly RestContext _restContext;

        public RegistrationRepository(RestContext restContext)
        {
            _restContext = restContext;
        }
        public async Task<IEnumerable<Registration>> GetAll(int timeId)
        {
            return await _restContext.Registrations.AsQueryable().Where(x => x.TimeId == timeId).ToListAsync();
        }
        public async Task<PagedList<Registration>> GetAll(RegistrationSearchPrams registrationSearchPrams)
        {
            var queryable = _restContext.Registrations.AsQueryable().OrderBy(o => o.startDate).Where(o => o.TimeId == registrationSearchPrams.timeId);
            return await PagedList<Registration>.CreateAsync(queryable, registrationSearchPrams.pageNumber, registrationSearchPrams.pageSize);
        }
        public async Task<Registration> Get(int id)
        {
            return await _restContext.Registrations.FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task Create(Registration registration)
        {
            _restContext.Registrations.Add(registration);
            await _restContext.SaveChangesAsync();
        }
        public async Task Update(Registration registration)
        {
            _restContext.Registrations.Update(registration);
            await _restContext.SaveChangesAsync();
        }
        public async Task Delete(Registration registration)
        {
            _restContext.Registrations.Remove(registration);
            await _restContext.SaveChangesAsync();
        }
    }
}
