using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class RevokedToken
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime RevokedAt { get; set; }

    public DateTime ExpiryDate { get; set; }

    public virtual User User { get; set; } = null!;
}
