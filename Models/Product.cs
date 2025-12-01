namespace ProductApp.Models
{
    public class Product
    {
        public int Id { get; set; }             // PK
        public string Name { get; set; } = "";  // Nome do produto
        public string? Description { get; set; }// Descrição opcional
        public decimal Price { get; set; }      // Preço
        public int Stock { get; set; }          // Estoque
        public DateTime CreatedAt { get; set; } // Data de criação
    }
}
