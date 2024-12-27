namespace CaseOpener.Core.Models.User
{
    /// <summary>
    /// Model for information about Role
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// Unique identifier for the role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
