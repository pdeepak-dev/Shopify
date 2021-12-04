namespace Discount.API.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}