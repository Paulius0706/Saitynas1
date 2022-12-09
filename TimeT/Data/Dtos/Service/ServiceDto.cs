using System.ComponentModel.DataAnnotations;

namespace TimeT.Data.Dtos.Service
{
    public class ServiceDto
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public bool isActice { get; set; }
        public bool isPublic { get; set; }
        public string description { get; set; }
    }
    //public record ServiceDto(int userId, string name, bool isActive, bool isPublic, string description);
}
