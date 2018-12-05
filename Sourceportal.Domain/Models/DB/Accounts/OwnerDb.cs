namespace Sourceportal.Domain.Models.DB.Accounts
{
    public class OwnerDb
    {
        public string OwnerId { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerImageURL { get; set; }
        public string ExternalID { get; set; }
        public decimal Percent { get; set; }
    }
}