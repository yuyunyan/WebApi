namespace Sourceportal.Domain.Models.Shared
{
    public static class DocumentPaths
    {
        public static string InspectionAnswerImages { get { return "Inspections/{0}/Answers/{1}"; } }
        public static string InspectionAdditionalImages{ get { return "Inspections/{0}/AdditionalImages"; } }
        public static string UploadedDocuments { get { return "Uploaded/{0}/{1}"; } }
        public static string BomUpload { get { return "BomUploads"; } }
    }
}
