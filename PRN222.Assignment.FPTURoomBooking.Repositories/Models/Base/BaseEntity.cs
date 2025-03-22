namespace PRN222.Assignment.FPTURoomBooking.Repositories.Models.Base;

/// <summary>
/// Base entity class that implements basic entity properties
/// </summary>
/// <typeparam name="TKey">Type of the primary key</typeparam>
public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    public virtual TKey Id { get; set; } 
}

/// <summary>
/// Auditable base entity class that implements both entity and auditable properties
/// </summary>
/// <typeparam name="TKey">Type of the primary key</typeparam>
public abstract class AuditableEntity<TKey> : BaseEntity<TKey>, IAuditable, ISoftDeletable
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public abstract class BaseEntity : BaseEntity<Guid>;

public abstract class AuditableEntity : AuditableEntity<Guid>;