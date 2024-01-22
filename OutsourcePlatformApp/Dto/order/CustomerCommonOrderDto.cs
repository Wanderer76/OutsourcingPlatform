using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto.order;

public class CustomerCommonOrderDto : CommonOrderDto
{
    public bool IsCompleted { get; set; }
    public int WorkersCount { get; set; }

    public CustomerCommonOrderDto(int orderId, int price, string name, string description, string companyName,
        int? responseCount, DateOnly deadline, bool isResponsed, IEnumerable<OrderVacancyDto> orderVacancies,
        OrderSkillsDto orderSkillsDto,
        OrderCategoriesDto? orderCategories, bool isCompleted, int workersCount)
        : this(orderId, price, name, description, companyName, responseCount, deadline, isResponsed, isCompleted,
            workersCount,
            orderVacancies, orderSkillsDto, orderCategories)
    {
        IsCompleted = isCompleted;
        WorkersCount = workersCount;
    }

    public CustomerCommonOrderDto(int orderId, int price, string name, string description, string companyName,
        int? responseCount, DateOnly deadline, bool isResponsed, bool isCompleted, int workersCount,
        IEnumerable<OrderVacancyDto> orderVacancies, OrderSkillsDto orderSkillsDto, OrderCategoriesDto? orderCategories)
        : base(orderId, price, name, description, companyName, responseCount, deadline, isResponsed, isCompleted,
            orderVacancies, orderSkillsDto, orderCategories)
    {
        WorkersCount = workersCount;
    }
}