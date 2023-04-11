namespace EasyBills.Core.Entity;

/// <summary>
/// Entity base class.
/// </summary>
public abstract class Entity : IEntity
{
    /// <summary>
    /// Entity id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Check if the entity is equals to other object.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <returns>If the objects are equal.</returns>
    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;

        if (ReferenceEquals(null, compareTo))
            return false;

        return Id.Equals(compareTo.Id);
    }

    /// <summary>
    /// Equals operator.
    /// </summary>
    /// <param name="a">Entity a.</param>
    /// <param name="b">Entity b.</param>
    /// <returns>If the entities are equal.</returns>
    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// Not equal operator.
    /// </summary>
    /// <param name="a">Entity a.</param>
    /// <param name="b">Entity b.</param>
    /// <returns>If the entities are not equal.</returns>
    public static bool operator !=(Entity a, Entity b) => !(a == b);

    /// <summary>
    /// Get a string from the entity.
    /// </summary>
    /// <returns>Entity converted to string</returns>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    /// <summary>
    /// Get hash code.
    /// </summary>
    /// <returns>Hash code.</returns>
    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }
}
