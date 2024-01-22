using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class ExecutorDetailCloseDto
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? SecondName { get; set; }
    public string? About { get; set; }
    public DetailExecutorDto Executor { get; set; }
    public bool IsClose { get; set; } = true;

    public ExecutorDetailCloseDto(User user, int finishProjectCount)
    {
        Surname = user.Surname;
        Name = user.Name;
        SecondName = user.SecondName;
        UserName = user.Username;
        About = user.UserContacts?.About ?? string.Empty;
        Executor = new DetailExecutorDto(user.Executor, finishProjectCount);
        Executor.Educations = null;
    }
}