namespace OutsourcePlatformApp.Dto;

public class ExecutorListResponse
{
    public int Count { get; set; }
    public List<CommonExecutorDto> Executors { get; set; }

    public ExecutorListResponse(List<CommonExecutorDto> executors, int count)
    {
        Executors = executors;
        Count = count;
    }
}