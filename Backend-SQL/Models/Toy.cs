using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Backend_SQL.Models
{
    [Table("toys")]
    public partial class Toy
    {
        [Key]
        [Column("ToyID")]
        public int ToyID { get; set; }

        [Column("Name")]
        public required string Name { get; set; }


        [Column("Quantity")]
        public int Quantity { get; set; }

        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();


    }

}
