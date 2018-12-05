using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Telerik.Reporting;

namespace Sourceportal.Reports
{
    public class Utilities
    {
        private static readonly string DocumentsDirectoryName = WebConfigurationManager.AppSettings["DocumentsFolder"];
        //private static void SaveReportToFolder(HttpPostedFile file, int inspectionId, string fileNameToSave)
        //{
        //    var fullRelativePath = "~/Reports/" + inspectionId + "/";

        //    var fullDirectoryPath = HttpContext.Current.Server.MapPath(fullRelativePath);

        //    if (!Directory.Exists(fullDirectoryPath))
        //    {
        //        Directory.CreateDirectory(fullDirectoryPath);
        //    }

        //    var filePath = HttpContext.Current.Server.MapPath(fullRelativePath + "/" + fileNameToSave);

        //    file.SaveAs(filePath);

        //}

        public static string ExportReport(string reportAssembly, string fileNameSuffix, Parameter[] paramList)
        {
            Telerik.Reporting.Processing.ReportProcessor reportProcessor =
            new Telerik.Reporting.Processing.ReportProcessor();

            // set any deviceInfo settings if necessary
            System.Collections.Hashtable deviceInfo =
                new System.Collections.Hashtable();

            Telerik.Reporting.TypeReportSource typeReportSource =
                         new Telerik.Reporting.TypeReportSource();

            //Add parameters to report
            foreach(Parameter param in paramList)
            {
                typeReportSource.Parameters.Add(param);
            }

            // reportName is the Assembly Qualified Name of the report
            typeReportSource.TypeName = reportAssembly;

            Telerik.Reporting.Processing.RenderingResult result =
                reportProcessor.RenderReport("PDF", typeReportSource, deviceInfo);
 
            //Prepare temp path
            string fileName = result.DocumentName + "_" + fileNameSuffix + "." + result.Extension;
            string filePath = HttpContext.Current.Server.MapPath("~/" + DocumentsDirectoryName + "/Export/");
            string fullFilePath = System.IO.Path.Combine(filePath, fileName);

            //Check if directory exists before uploading to documents folder
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //Check if new file exists before creating it
            if(File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
            }
            
            //Save new file from temp
            using (System.IO.FileStream fs = new System.IO.FileStream(fullFilePath, System.IO.FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }

            return DocumentsDirectoryName + "/Export/" + fileName;
        }

        public static int MergePDF(string[] filePaths, string newfileName, out string mergedFilePath)
        {
            //prepare output path
            string outputDir = DocumentsDirectoryName + "/Export/Merged";
            string outputPath = System.IO.Path.Combine(outputDir, newfileName + ".pdf");
            string fullOutputPath = HttpContext.Current.Server.MapPath("~/" + outputPath);

            //Check if directory exists before uploading to documents folder
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            //Check if new file exists before creating it
            if (File.Exists(fullOutputPath))
            {
                System.IO.File.Delete(fullOutputPath);
            }

            // Merge PDF files		
            SautinSoft.PdfVision v = new SautinSoft.PdfVision();
            int ret = v.MergePDFFileArrayToPDFFile(filePaths, fullOutputPath);

            //output file
            if (ret == 0)
                mergedFilePath = outputPath;
            else mergedFilePath = null;

            return ret;
            //0 - merged successfully
            //1 - error, can't merge PDF documents
            //2 - error, can't create output file, probably it used by another application
            //3 - merging failed
            //4 - merged successfully, but some files were not merged
        }
    }
}
