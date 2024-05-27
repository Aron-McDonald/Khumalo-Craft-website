using System.ComponentModel.DataAnnotations.Schema;

namespace CLDV_POE_PART_2.Models
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("Client")]
        public string ClientId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public DateTime OrderDate { get; set; }

        public ApplicationUser Client { get; set; }
        public Product Product { get; set; }
    }
}
