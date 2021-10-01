using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Utils;

namespace AuthGold.Providers
{
    public static  class FileProvider
    {
        public static string ReadPDF(string pdfPath)
        {
            var pageText = new StringBuilder();
            using(PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfPath)))
            {
                var pageNumbers = pdfDocument.GetNumberOfPages();
                for(int i = 1; i <= pageNumbers; i++)
                {
                    LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                    parser.ProcessPageContent(pdfDocument.GetFirstPage());
                    pageText.Append(strategy.GetResultantText());
                }
            }
            return pageText.ToString();
        }

        public static void MergeFiles(string targetDirectory) {
            string dest = targetDirectory +  @"\Merged.pdf";
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            
            using(PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest)))
            {
                PdfMerger merger = new PdfMerger(pdfDoc);

                foreach (string fileName in fileEntries) {
                    PdfDocument newDoc = new PdfDocument(new PdfReader(fileName));
                    //PdfMerger merger = new PdfMerger(pdfDoc);
                    merger.Merge(newDoc, 1, newDoc.GetNumberOfPages());
                };
            }
        }

        public static void GetInfoPDF(string filepath)
        {
            using(PdfDocument pdf = new PdfDocument(new PdfReader(filepath)))
            {
                Console.WriteLine(pdf.GetNumberOfPages());
                Console.WriteLine(pdf.GetEncryptedPayloadDocument().ToString());
                Console.WriteLine(pdf.GetDocumentInfo().GetAuthor());
            }
        }
    }
}

