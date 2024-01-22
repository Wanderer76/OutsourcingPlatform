namespace OutsourcePlatformApp.Dto;

public class ReviewCreationDto
{
    public int ExecutorId { get; set; }
    public int OrderId { get; set; }
    public string ReviewText { get; set; }
}