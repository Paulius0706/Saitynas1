namespace TimeT.Data.Dtos.Service
{
    public class ServiceSearchParam
    {
        private int _pageSize = 5;
        public const int maxPageSize = 50;


        public int pageNumber { get; set; } = 1;
        
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }
    }
}
