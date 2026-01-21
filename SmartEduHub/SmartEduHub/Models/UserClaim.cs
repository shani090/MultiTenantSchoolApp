using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class UserClaim
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
