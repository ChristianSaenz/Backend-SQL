using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Backend_SQL.Models
{
    [Table("orders")]
    public partial class Order
    {
        [Key]
        [Column("OrderID")]
        public int OrderID { get; set; }

        [Column("FirstName")]
        public required string FirstName { get; set; }

        [Column("LastName")]
        public required string LastName { get; set; }
        
        [ForeignKey("Toy")]
        [Column("ToyID")]
        public int? ToyID { get; set; }
        public Toy? Toy { get; set; }

        [ForeignKey("Cloth")]
        [Column("ClothID")]
        public int ClothID { get; set; }
        public Cloth? Cloth { get; set; }

        [Column("Email")]
        public required string Email { get; set; }

        [Column("OrderNumber")]
        public int OrderNumber { get; set; }

        [Column("OrderDate")]
        public required DateOnly OrderDate { get; set; }




    }

}
