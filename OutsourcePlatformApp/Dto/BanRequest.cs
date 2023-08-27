namespace OutsourcePlatformApp.Dto;

public class BanRequest
{
    public int UserId { get; set; }
    public bool IsBanned { get; set; }
    public string? Message { get; set; }
}