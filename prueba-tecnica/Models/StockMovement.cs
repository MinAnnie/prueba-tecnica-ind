namespace prueba_tecnica.Models;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }
}