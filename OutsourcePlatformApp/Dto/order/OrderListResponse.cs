namespace OutsourcePlatformApp.Dto.order;

public class OrderListResponse
{
    public int Count { get; set; }
    public List<CommonOrderDto> Orders { get; set; }

    public OrderListResponse(List<CommonOrderDto> orders, int count)
    {
        Orders = orders;
        Count = count;
    }
}