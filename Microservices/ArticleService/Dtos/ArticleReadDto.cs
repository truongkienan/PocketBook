namespace ArticleService.Dtos
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public byte ColumnIndex { get; set; }
        public short Position { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
    }
}