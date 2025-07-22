﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using P2Aspire.Core.Security;


namespace P2Aspire.Data.Abstractions.Entity;
public record Sample
{

    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    [Secure]
    [Column( TypeName = "bytea" )]
    public required string Description { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

}
