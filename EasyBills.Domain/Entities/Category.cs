﻿using EasyBills.Core.Entity;

namespace EasyBills.Domain.Entities;

/// <summary>
/// Represents a category entity.
/// </summary>
public class Category : Entity
{
    /// <summary>
    /// Initialize a new instance of <see cref="Category"/> class.
    /// </summary>
    public Category(string name, string icon, string color, Guid? userId, string? description)
    {
        Name = name;
        Icon = icon;
        Color = color;
        UserId = userId;
        Description = description;
        Transactions = new List<Transaction>();
    }

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Category icon.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Category icon.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// User id.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Transactions.
    /// </summary>
    public List<Transaction> Transactions { get; set; }
}
