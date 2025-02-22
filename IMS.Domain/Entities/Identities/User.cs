using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities.Identities
{
    public class User : IdentityUser<int>
    {
        public string? Code { get; set; }
        public virtual ICollection<Transaction> ListOfTransactions { get; set; }
        [InverseProperty(nameof(UserRefreshToken.user))]
        public virtual ICollection<UserRefreshToken> userRefreshToken { get; set; }

    }
}
