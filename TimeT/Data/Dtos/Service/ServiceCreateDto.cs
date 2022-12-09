using System.ComponentModel.DataAnnotations;

namespace TimeT.Data.Dtos.Service
{
    public class ServiceCreateDto
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }

    }
}
