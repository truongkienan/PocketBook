namespace CategoryService.Dtos
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { set; get; }
        public int? ParentCategoryId { get; set; }
    }
}
