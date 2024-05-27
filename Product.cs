namespace CLDV_POE_PART_2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Price { get; set; } = "";
        public string Category { get; set; } = "";
        public string ImageFileName { get; set; } = "";
        public string Availability { get; set; } = "";

        public ICollection<Order> Orders { get; set; }

    }
}
