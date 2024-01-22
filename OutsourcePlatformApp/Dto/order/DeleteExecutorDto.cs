namespace OutsourcePlatformApp.Dto.order;

public class DeleteExecutorDto
{
    public int ExecutorId { get; set; }
    public int OrderId { get; set; }
    public string Message { get; set; } = string.Empty;
}