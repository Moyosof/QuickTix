using System.ComponentModel.DataAnnotations;

namespace QuickTix.API.Entities.Core
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string TicketName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        [Range(1, 10000)]
        public double Price { get; set; }
        [Display(Name = "ImageUrl")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
