using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreateOn { get; set; } = DateTime.Now;

        // (1-x) Relation
        public int? StockId { get; set; }

        [ForeignKey("StockId")]
        public Stock? Stock { get; set; }
    }
}
