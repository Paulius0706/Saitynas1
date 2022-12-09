using System.ComponentModel.DataAnnotations;

namespace TimeT.Data.Entities
{
    public class Service
    {
        public int id { get; set; }

        [Required]
        public string userId { get; set; }
        public string name { get; set; }
        public bool isActice { get; set; }
        public bool isPublic { get; set; }
        public string description { get; set; }

    }
}
