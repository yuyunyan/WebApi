namespace Sourceportal.Domain.Models.API.Requests.Accounts
{
    public class ContactsFilter
    {
        public string FreeTextSearch { get; set; }
        public int AccountId { get; set; }
        public int ContactId { get; set; }
        public int RowOffset { get; set; }
        public int RowLimit { get; set; }
        public int DescSort { get; set; }
        public string SortBy { get; set; }
        public string FilterBy { get; set; }
        public string FilterText { get; set; }
        public int AccountTypeId { get; set; }
        public bool AccountIsActive { get; set; }
    }
}