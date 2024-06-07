namespace WebApplication2.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public string InventoryDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
