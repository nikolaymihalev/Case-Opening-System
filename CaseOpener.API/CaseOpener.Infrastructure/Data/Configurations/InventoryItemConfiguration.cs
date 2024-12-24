using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.InventoryItems).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Item).WithMany(x => x.InventoryItems).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
