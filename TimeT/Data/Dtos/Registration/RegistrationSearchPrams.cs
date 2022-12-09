namespace TimeT.Data.Dtos.Registration
{
    public class RegistrationSearchPrams
    {
        private int _pageSize = 5;
        public const int maxPageSize = 50;


        public int pageNumber { get; set; } = 1;
        public int timeId { get; set; }

        public int pageSize
        {
            get{return _pageSize;}
            set{ _pageSize = value> maxPageSize ? maxPageSize : value;}
        }
    }
}
