using System.ComponentModel.DataAnnotations.Schema;

namespace TimeT.Data.Entities
{
    public class Time
    {
        public int id { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string type { get; set; }
        public bool visibility { get; set; }
        public bool register1 { get; set; }
        public bool register2 { get; set; }
        public int maxRegisterTime { get; set; }
        public int sectorInterval1 { get; set; }
        public int sectorInterval2 { get; set; }
        
        [ForeignKey("Service")]
        public int serviceId { get; set; }
    }
}
