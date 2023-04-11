namespace EasyBills.Core.Entity;

/// <summary>
/// Entity interface.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; }
}
