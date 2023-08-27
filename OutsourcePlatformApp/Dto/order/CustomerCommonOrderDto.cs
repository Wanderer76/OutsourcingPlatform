using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto.order;

public class CustomerCommonOrderDto : CommonOrderDto
{
    public bool IsCompleted { get; set; }

    public CustomerCommonOrderDto(int orderId, int price, string name, string description, int maxWorkers, string companyName, int? responseCount, DateOnly deadline, bool isResponsed, OrderSkillsDto? orderSkills, OrderCategoriesDto? orderCategories,bool isCompleted)
    :this( orderId, price, name,  description, maxWorkers, companyName,  responseCount, deadline, isResponsed, isCompleted, orderSkills,  orderCategories)
    {
        IsCompleted = isCompleted;
    }
    
    public CustomerCommonOrderDto(int orderId, int price, string name, string description, int maxWorkers, string companyName, int? responseCount, DateOnly deadline, bool isResponsed, bool isCompleted ,OrderSkillsDto? orderSkills, OrderCategoriesDto? orderCategories)
        : base(orderId, price, name, description, maxWorkers, companyName, responseCount, deadline, isResponsed, isCompleted,orderSkills, orderCategories)
    {
    }
}