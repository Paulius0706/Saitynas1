using System.ComponentModel.DataAnnotations;

namespace TimeT.Data.Dtos.Service
{
    public class ServiceUpdateDto
    {
        [Required]
        public string description { get; set; }
        public bool isActice { get; set; }
        public bool isPublic { get; set; }
    }
}
