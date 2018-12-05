
namespace Sourceportal.Domain.Models.DB.Accounts
{
   public class AccountFocusTypeDb
    {
        public int FocusTypeId { get; set; }
        public int TypeRank { get; set; }
        public bool IsBlacklisted { get; set; }
        public string FocusName { get; set; }
    }
}
