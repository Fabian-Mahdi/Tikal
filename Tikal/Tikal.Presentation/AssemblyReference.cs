using System.Reflection;

namespace Tikal.Presentation;

/// <summary>
///     Used to easily reference the assembly in other projects
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    ///     Gets the assembly of the current project
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}