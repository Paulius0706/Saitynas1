using TimeT.Data.Entities;

namespace TimeT.Data.Dtos.Time
{
    public class TimeDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool register1 { get; set; }
        public bool register2 { get; set; }
        public int sectorInterval1 { get; set; }
        public int sectorInterval2 { get; set; }
        public int serviceId { get; set; }
    }
}
