using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class CommonExecutorDto
{
    public int ExecutorId { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string SecondName { get; set; }
    public int? CompletedOrders { get; set; }
    public ExecutorSkillsDto? ExecutorSkills { get; set; }
    public ExecutorCategoriesDto? ExecutorCategories { get; set; }

    public CommonExecutorDto(int executorId, string username, string name,
        string surname,
        string? secondName, int? completedOrders, ExecutorSkillsDto? skills, ExecutorCategoriesDto? categories)
    {
        ExecutorId = executorId;
        Username = username;
        CompletedOrders = completedOrders;
        ExecutorSkills = skills;
        ExecutorCategories = categories;
        Name = name;
        Surname = surname;
        SecondName = secondName ?? string.Empty;
    }
}