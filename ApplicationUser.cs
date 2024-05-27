using Microsoft.AspNetCore.Identity;

namespace CLDV_POE_PART_2.Models
{
public class ApplicationUser : IdentityUser
{
        public ICollection<Order> Orders { get; set; }
        public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public string Address { get; set; } = "";

    public DateTime CreatedAt { get; set; }


}
}
