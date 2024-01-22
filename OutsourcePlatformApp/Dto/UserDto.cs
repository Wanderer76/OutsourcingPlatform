namespace OutsourcePlatformApp.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string SecondName { get; set; }
    public string Surname { get; set; }
    public string UserRole { get; set; }
    public bool isBanned { get; set; }
}