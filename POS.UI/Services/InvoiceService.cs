
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace POS.UI.Services
{
    public class InvoiceService
    {
        public PdfDocument GenerateSaleReceipt(string saleNumber, string storeName, string address, string email,
           string clientName, string clientDocument, List<ProductItem> products,
           string subTotal, string taxes, string total)
        {
            // Create a new PDF document
            var document = new Document();

            // Add a section to the document
            Section section = document.AddSection();

            // Set smaller margins for more compact layout
            section.PageSetup.LeftMargin = 30;
            section.PageSetup.RightMargin = 30;


            // ========== 1. LOGO ==========
            var image = section.Headers.Primary.AddImage("wwwroot/assets/companyLogoImage/POS-feature.png"); // Update path
            image.LockAspectRatio = true;
            image.Width = "3cm";
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Left;

            // ========== 3. SALE INFO ==========
            var saleTable = section.AddTable();
            //saleTable.Borders.Width = 0.75; // Adds visible borders to see layout
            saleTable.AddColumn("13cm");
            saleTable.AddColumn("6cm");
            
            var row1 = saleTable.AddRow();
            row1.BottomPadding = 10;
            // "SALE NUMBER" text
            var saleNumberTitle = row1.Cells[1].AddParagraph("SALE NUMBER");
            saleNumberTitle.Format.Font.Color = Colors.Teal;
            saleNumberTitle.Format.Font.Size = 16; // Set font size
            saleNumberTitle.Format.Alignment = ParagraphAlignment.Right; // Align right
            var saleNumberVal = row1.Cells[1].AddParagraph(saleNumber);
            saleNumberVal.Format.Font.Bold = true;
            saleNumberVal.Format.Alignment = ParagraphAlignment.Right;

            var row2 = saleTable.AddRow();
            
            row2.TopPadding = 15;
            row2.BottomPadding = 30;
            row2.Cells[0].AddParagraph().AddFormattedText("Example Store\n", TextFormat.Bold);
            var storeAddress = row2.Cells[0].AddParagraph(address);
            storeAddress.Format.Font.Color = Colors.Gray;
            storeAddress.Format.Alignment = ParagraphAlignment.Left;

            var storeEmail = row2.Cells[0].AddParagraph(email);
            storeEmail.Format.Font.Color = Colors.Gray;
            storeEmail.Format.Alignment = ParagraphAlignment.Left;

            var cliennt = row2.Cells[1].AddParagraph("CLIENT");
            cliennt.Format.Font.Bold = true;
            cliennt.Format.Alignment = ParagraphAlignment.Right;

            var clientNamee = row2.Cells[1].AddParagraph(clientName);
            clientNamee.Format.Font.Color = Colors.Gray;
            clientNamee.Format.Alignment = ParagraphAlignment.Right;

            var clientDocc = row2.Cells[1].AddParagraph(clientDocument);
            clientDocc.Format.Font.Color = Colors.Gray;
            clientDocc.Format.Alignment = ParagraphAlignment.Right;


            

            // ========== 4. PRODUCT TABLE ==========
            var table = section.AddTable();
            table.Format.SpaceBefore = "0.3cm";
            table.Borders.Width = 0.5;
            table.Borders.Color = Colors.LightGray;

            table.AddColumn("10cm");
            table.AddColumn("2cm");
            table.AddColumn("3.5cm");
            table.AddColumn("3.5cm");

            var header = table.AddRow();
            header.TopPadding = 0;
            header.BottomPadding = 5;
            header.Shading.Color = Colors.Teal;
            header.Format.Font.Color = Colors.White;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("Product");
            header.Cells[1].AddParagraph("Quantity");
            header.Cells[2].AddParagraph("Price");
            header.Cells[3].AddParagraph("Total");

            foreach (var p in products)
            {
                var row = table.AddRow();
                row.BottomPadding = 5;
                row.TopPadding = 0;
                row.Cells[0].AddParagraph(p.Name);
                row.Cells[1].AddParagraph(p.Quantity.ToString());
                row.Cells[2].AddParagraph($"Rs {p.Price:0.00}");
                row.Cells[3].AddParagraph($"Rs {p.Total:0.00}");
            }

            // ========== 5. TOTAL SUMMARY ==========
            var summary = section.AddTable();
            summary.Borders.Visible = true;
            summary.Borders.Width = 0.5;
            summary.Borders.Color = Colors.LightGray;
            summary.AddColumn("12cm");
            summary.AddColumn("3.5cm");
            summary.AddColumn("3.5cm");

            summary.Format.Alignment = ParagraphAlignment.Right;
            summary.Format.SpaceBefore = "0.3cm";

            //void AddSummaryRow(string label, string amount, bool bold = false, bool highlight = false)
            //{
            //    var r = summary.AddRow();
            //    r.BottomPadding = 5;
            //    r.TopPadding = 0;
            //    r.Cells[1].AddParagraph(label);
            //    var amt = r.Cells[2].AddParagraph($"USD {amount:0.00}");
            //    if (bold)
            //        amt.Format.Font.Bold = true;
            //    if (highlight)
            //    {
            //        r.Cells[2].Shading.Color = Colors.Teal;
            //        amt.Format.Font.Color = Colors.White;
            //    }
            //}
            void AddSummaryRow(string label, string amount, bool bold = false, bool highlight = false)
            {
                var r = summary.AddRow();
                r.BottomPadding = 5;
                r.TopPadding = 0;

                // Hide borders for the first column
                r.Cells[0].Borders.Visible = false;

                // Continue with normal formatting
                r.Cells[1].AddParagraph(label);
                var amt = r.Cells[2].AddParagraph($"Rs {amount:0.00}");

                if (bold)
                    amt.Format.Font.Bold = true;

                if (highlight)
                {
                    r.Cells[2].Shading.Color = Colors.Teal;
                    amt.Format.Font.Color = Colors.White;
                }
            }


            AddSummaryRow("Sub Total", subTotal, highlight: true);
            AddSummaryRow("Total Taxes", taxes, highlight: true);
            AddSummaryRow("Total", total, bold: true, highlight: true);


            // Render the document
            var pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }
    }

    public class ProductItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
