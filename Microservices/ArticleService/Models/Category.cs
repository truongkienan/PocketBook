using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleService.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
     }
}