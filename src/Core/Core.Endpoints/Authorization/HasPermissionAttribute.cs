using Microsoft.AspNetCore.Authorization;

namespace Core.Endpoints.Authorization
{
    /// <summary>
    /// Specifies that the method that this attribute is applied to requires the specified permission.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class.
    /// </remarks>
    /// <param name="permission">The permission.</param>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {

        /// <summary>
        /// Gets the permission.
        /// </summary>
        public string Permission { get; } = permission;
    }
}
