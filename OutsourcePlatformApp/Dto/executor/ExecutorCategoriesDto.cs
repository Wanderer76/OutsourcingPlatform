using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class ExecutorCategoriesDto
{
    public List<CategoryDto>? Categories { get; set; }

    public ExecutorCategoriesDto(Executor executor)
    {
        if(executor.Categories != null)
            Categories = executor.Categories.Select(category => new CategoryDto{ Id = category.Id, Name = category.Name }).ToList();
    }
}