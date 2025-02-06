namespace Backend_SQL.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ToyID { get; set; }
        public int? ClothID { get; set; }
        public string Email { get; set; }
        public int OrderNumber { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? ToyName { get; set; }
        public string? ClothName { get; set; }
    }
}
