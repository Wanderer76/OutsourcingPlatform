using Shared.Entities;

namespace OutsourcePlatformApp.Models;

public class ContactLink : BaseEntity
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int ContactId { get; set; }
    public Contact Contact { get; set; }
}