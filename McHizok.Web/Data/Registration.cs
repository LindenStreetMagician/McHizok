﻿using System.ComponentModel.DataAnnotations.Schema;

namespace McHizok.Web.Data;

[Table("Registration")]
public class Registration
{
    public Guid Id { get; set; }
    public string RegistrationToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string AccountFor { get; set; }
}
