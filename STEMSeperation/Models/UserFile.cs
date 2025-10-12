using System;
using System.Collections.Generic;

namespace PresentationLayer.Models;

public partial class UserFile
{
    public int InstanceId { get; set; }

    public string InputPath { get; set; } = null!;

    public int UserId { get; set; }

    public string? Stem1 { get; set; }

    public string? Stem2 { get; set; }

    public string? Stem3 { get; set; }

    public string? Stem4 { get; set; }

    public string? Stem5 { get; set; }

    public DateTime InstanceTime { get; set; }

    public virtual User User { get; set; } = null!;
}
