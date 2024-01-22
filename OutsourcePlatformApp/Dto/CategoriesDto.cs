using OutsourcePlatformApp.Models;


namespace OutsourcePlatformApp.Dto
{
    public class CategoriesDto
    {
        public List<CategoryDto>? Categories { get; set; }

        public CategoriesDto(Executor executor)
        {
            if(executor.Categories != null)
                Categories = executor.Categories.Select(category => new CategoryDto{Id = category.Id, Name = category.Name}).ToList();
        }
    }
}