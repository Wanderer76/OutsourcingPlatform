using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Entities;

namespace OutsourcePlatformApp.Models;

public class ActivationToken : BaseEntity
{
    public User User { get; set; }
    public int UserId { get; set; }
    public Guid Token { get; set; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; } = DateTimeOffset.UtcNow.AddHours(1);
    public bool IsActivated { get; set; }
}