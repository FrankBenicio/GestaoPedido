
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoPedido.Domain.Models;
public abstract class EntityBase
{
    public Guid Id { get; private set; }

    public Guid CreatedBy { get; private set; }

    [ForeignKey(nameof(CreatedBy))]
    public User? CreatedByUser { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    [ForeignKey(nameof(UpdatedBy))]
    public User? UpdatedByUser { get; private set; }

    public DateTime? UpdatedOn { get; private set; }

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }

    public void SetCreated(Guid userId)
    {
        CreatedBy = userId;
        CreatedOn = DateTime.UtcNow;
    }

    public void SetUpdated(Guid userId)
    {
        UpdatedBy = userId;
        UpdatedOn = DateTime.UtcNow;
    }
}

