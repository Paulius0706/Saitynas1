using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TimeT.Auth.Model;
using TimeT.Data.Entities;
using TimeT.Data.Dtos.Service;

namespace TimeT.Data
{
    public class RestContext : IdentityDbContext<TimeTUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Service> Services { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        public RestContext(IConfiguration configuration)
        {
            _configuration = configuration;
          
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(_configuration.GetValue<string>("PostgreSQLConnectionString"));
        //}

        public DbSet<TimeT.Data.Dtos.Service.ServiceDto> ServiceDto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433; Database=saitynas; User=sa; Password =Pasword1-;");
        }
    }
}
