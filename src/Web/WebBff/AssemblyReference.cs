using System.Reflection;

namespace WebBff
{
    /// <summary>
    /// Represents the users module infrastructure assembly reference.
    /// </summary>
    public static class AssemblyReference
    {
        /// <summary>
        /// The assembly.
        /// </summary>
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
