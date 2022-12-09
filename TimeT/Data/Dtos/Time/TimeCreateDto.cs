using System.ComponentModel.DataAnnotations;

namespace TimeT.Data.Dtos.Time
{
    public class TimeCreateDto
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string startDate { get; set; }
        [Required]
        public string endDate { get; set; }
        [Required]
        public string type { get; set; }
        public bool visibility { get; set; }
        public bool register1 { get; set; }
        public bool register2 { get; set; }
        [Required] 
        public int maxRegisterTime { get; set; }
        public int sectorInterval1 { get; set; }
        public int sectorInterval2 { get; set; }
        public int serviceId { get; set; }
    }
}
