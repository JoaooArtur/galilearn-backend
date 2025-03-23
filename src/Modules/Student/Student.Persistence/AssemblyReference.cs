using System.Reflection;

namespace Student.Persistence
{
    /// <summary>
    /// Represents the students module infrastructure assembly reference.
    /// </summary>
    public static class AssemblyReference
    {
        /// <summary>
        /// The assembly.
        /// </summary>
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
