using System;
using System.Collections.Generic;

namespace practicasumativasintesis.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<DetallesVenta> DetallesVenta { get; set; } = new List<DetallesVenta>();
}
