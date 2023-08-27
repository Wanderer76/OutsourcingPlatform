using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class CommonExecutorDto
{
    public int ExecutorId { get; set; }
    public string Username { get; set; }
    public int? CompletedOrders { get; set; }
    public ExecutorSkillsDto? ExecutorSkills { get; set; }
    public ExecutorCategoriesDto? ExecutorCategories { get; set; }

    public CommonExecutorDto(int executorId, string username, int? completedOrders, ExecutorSkillsDto? skills, ExecutorCategoriesDto? categories)
    {
        ExecutorId = executorId;
        Username = username;
        CompletedOrders = completedOrders;
        ExecutorSkills = skills;
        ExecutorCategories = categories;
    }
}