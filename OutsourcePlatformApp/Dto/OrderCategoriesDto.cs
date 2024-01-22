using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class OrderCategoriesDto
{
    public List<CategoryDto>? Categories { get; set; }

    public OrderCategoriesDto(Order order)
    {
        if(order.OrderCategories != null)
            Categories = order.OrderCategories.Select(category => new CategoryDto{Id = category.Id, Name = category.Name}).ToList();
    }
}