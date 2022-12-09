using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeT.Data.Entities
{
    public class Registration
    {
        public int id { get; set; }
        [Required]
        public string userId { get; set; }
        public string type { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string comment { get; set; }
        
        [ForeignKey("Time")]
        public int TimeId { get; set; }
    }
}
