using System;
using System.Collections.Generic;

namespace DatabaseLayerLogic.Models;

public partial class User
{
    public int Pkuser { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string SaltValue { get; set; } = null!;

    public DateTime? UserCreatedOn { get; set; }

    public DateTime? LastLogIn { get; set; }

    public DateTime? LastPasswordChange { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();
}
