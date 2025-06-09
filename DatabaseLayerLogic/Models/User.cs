using System;
using System.Collections.Generic;

namespace DatabaseLayerLogic.Models;

public partial class User
{
    public int Pkuser { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime UserCreatedOn { get; set; }

    public DateTime? LastLogIn { get; set; }
}
