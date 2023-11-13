using System.ComponentModel.DataAnnotations;

namespace ArticleService.Dtos
{
    public class ArticleCreateDto
    {        
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Content { get; set; }
        public byte ColumnIndex { get; set; }
        public short Position { get; set; }
        public bool Active { get; set; }
    }
}