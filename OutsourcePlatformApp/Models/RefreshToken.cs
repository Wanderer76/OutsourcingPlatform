using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Entities;

namespace OutsourcePlatformApp.Models;

[Table("RefreshTokens")]
public class RefreshToken : BaseEntity
{
    public string Token { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly Expires { get; set; }

}