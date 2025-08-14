using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace practicasumativasintesis.Pdf
{
    public class DetalleVentasPdfDoc : IDocument
    {
        private List<DetalleVentasPdf> Model { get; }
        public DetalleVentasPdfDoc(List<DetalleVentasPdf> model)
        {
            Model = model;
        }
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Volumen de ventas")
                    .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(20);

                        column.Item().Text("Volumen de ventas");

                        // Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // FECHA
                                columns.RelativeColumn(2); // PRODUCTO
                                columns.RelativeColumn(2); // CANTIDAD
                                columns.RelativeColumn(2); // TOTAL
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("FechaVenta");
                                header.Cell().Element(CellStyle).Text("Producto");
                                header.Cell().Element(CellStyle).Text("Cantidad");
                                header.Cell().Element(CellStyle).Text("TOTAL");

                                static IContainer CellStyle(IContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                            });

                            // Rows
                            foreach (var item in Model)
                            {

                                table.Cell().Element(CellStyle).Text(item.FechaVenta.ToString("yyyy-MM-dd"));
                                table.Cell().Element(CellStyle).Text(item.NombreProducto);
                                table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                                table.Cell().Element(CellStyle).Text(item.Total.ToString("C"));

                                static IContainer CellStyle(IContainer container) =>
                                    container.PaddingVertical(5);
                            }
                        });
                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Página ");
                                x.CurrentPageNumber();
                            });
                    });
            });
        }
    }
}

