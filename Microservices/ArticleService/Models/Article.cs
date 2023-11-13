using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArticleService.Models
{
    [Table("Article")]
    public class Article
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public byte ColumnIndex { get; set; }
        public short Position { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
