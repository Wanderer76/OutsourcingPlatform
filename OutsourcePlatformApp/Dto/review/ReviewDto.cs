namespace OutsourcePlatformApp.Dto;

public class ReviewDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string CompanyName { get; set; }
    public string ProjectnName { get; set; }
    public DateTime ProjectCompleted { get; set; }
    public string Description { get; set; }
    public List<string> Competitences { get; set; }
}