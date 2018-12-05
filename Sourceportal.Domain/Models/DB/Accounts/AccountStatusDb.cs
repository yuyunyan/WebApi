namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class AccountStatusDb
    {
        public int AccountStatusId { get; set; }
        public string StatusName { get; set; }
        public string ExternalId { get; set; }
        public bool AccountIsActive { get; set; }
    }
}
