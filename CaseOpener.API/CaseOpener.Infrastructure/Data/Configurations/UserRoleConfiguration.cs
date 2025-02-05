using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseOpener.Infrastructure.Data.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            var seed = new Seed();

            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.HasOne(x => x.User).WithMany(x => x.UserRoles).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(seed.UserRoles);
        }
    }
}
