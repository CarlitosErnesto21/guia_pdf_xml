using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicasumativasintesis.Models;
using practicasumativasintesis.Pdf;
using QuestPDF.Fluent;

namespace practicasumativasintesis.Controllers
{
    public class ProductoController : Controller
    {
        private readonly PracticaSsmContext _context;

        public ProductoController(PracticaSsmContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Acción para generar el PDF de los detalles de ventas
        [HttpGet(Name = "DetalleVentasPdf")]
        public IResult DetalleVentasPdf(int n)
        {
            string sql = "SELECT v.fecha AS fecha_venta, p.nombre AS nombre_producto, dv.cantidad, dv.subtotal AS total FROM detalles_ventas dv INNER JOIN ventas v ON dv.venta_id = v.id INNER JOIN productos p ON dv.producto_id = p.id;";
            List<DetalleVentasPdf> data = _context.VentasPdf
                .FromSqlRaw(sql)
                .ToList();
            var document = new DetalleVentasPdfDoc(data);
            var pdfStream = document.GeneratePdf();
            return Results.File(pdfStream, "application/pdf", "ventas.pdf");
        }

        //Acción para generar el XLSX de los productos
        [HttpPost]
        public FileResult ExportarXLSX()
        {
            long id = Convert.ToInt32(Request.Form["id"]);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var productos = _context.Productos.ToList();

                IXLWorksheet ws = wb.Worksheets.Add();

                ws.Range("A1").Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                ws.Range("A1").Style.Font.Bold = true;
                ws.Range("A1").Style.Font.FontSize = 14;
                ws.Range("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("A1:E1").Merge();
                ws.Range("A3").Value = "ID";
                ws.Range("A3").Style.Font.Bold = true;
                ws.Range("B3").Value = "NOMBRE";
                ws.Range("B3").Style.Font.Bold = true;
                ws.Range("C3").Value = "DESCRIPCIÓN";
                ws.Range("C3").Style.Font.Bold = true;
                ws.Range("D3").Value = "PRECIO";
                ws.Range("D3").Style.Font.Bold = true;
                ws.Range("E3").Value = "STOCK";
                ws.Range("E3").Style.Font.Bold = true;

                int row = 4;
                foreach (Producto item in productos)
                {
                    ws.Cell(row, 1).Value = item.Id;
                    ws.Cell(row, 2).Value = item.Nombre;
                    ws.Cell(row, 3).Value = item.Descripcion;
                    ws.Cell(row, 4).Value = item.Precio;
                    ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell(row, 5).Value = item.Stock;
                    ws.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                    row++;
                }
                ws.Column(1).AdjustToContents();
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                ws.Column(4).AdjustToContents();
                ws.Column(5).AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
                }
            }
        }
    }
}
