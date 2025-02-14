using Microsoft.AspNetCore.Identity;

namespace IMS.Domain.Entities.Identities
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<Transaction> ListOfTransactions { get; set; }
    }
}
