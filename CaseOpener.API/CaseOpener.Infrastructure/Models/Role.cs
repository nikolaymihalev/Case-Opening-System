using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the user role
    /// </summary>
    [Comment("Represents the user role")]
    public class Role
    {
        /// <summary>
        /// Unique identifier for the role.
        /// </summary>
        [Key]
        [Comment("Role's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        [Required]
        [Comment("Role's name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of user roles.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
