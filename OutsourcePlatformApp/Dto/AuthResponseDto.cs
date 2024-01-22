namespace OutsourcePlatformApp.Dto;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    
    public string RefreshToken { get; set; }
    public string Role { get; set; }
}