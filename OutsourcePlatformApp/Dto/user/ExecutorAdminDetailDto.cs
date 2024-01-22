using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto.user;

public class ExecutorAdminDetailDto : ExecutorDetailOpenDto
{
    public bool IsBanned { get; set; }
    public string BannedMessage { get; set; }
    public int ProjectsCount { get; set; }
    public int FinishProjectCount { get; set; }
    public string Role { get; set; }
    

    public ExecutorAdminDetailDto(User user, int finishProjectCount,int projectsCount) : base(user, finishProjectCount)
    {
        BannedMessage = user.BannedMessage;
        ProjectsCount = projectsCount;
        FinishProjectCount = finishProjectCount;
        Role = user.UserRoles.First().Name;
        IsBanned = user.IsBanned;
    }
}