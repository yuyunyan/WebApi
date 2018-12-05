namespace Sourceportal.Domain.Models.DB.Items
{
    public class IhsItem
    {
        public string ObjectId { get; set; }
        public string PartNumber { get; set; }
        public string Manufacturer { get; set; }
        public string PartDescription { get; set; }
        public string PackageDescription { get; set; }
        public string ReachCompliant { get; set; }
        public string EuRohs { get; set; }
        public string CNRohs { get; set; }
        public string DatasheetURL { get; set; }
        public string PartStatus { get; set; }
        public string GenericNumber { get; set; }
        public string Category { get; set; }
    }
}
