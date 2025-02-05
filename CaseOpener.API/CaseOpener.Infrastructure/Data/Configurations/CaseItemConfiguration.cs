using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class CaseItemConfiguration : IEntityTypeConfiguration<CaseItem>
    {
        public void Configure(EntityTypeBuilder<CaseItem> builder)
        {
            builder.HasOne(x => x.Case).WithMany(x => x.CaseItems).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Item).WithMany(x => x.CaseItems).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
