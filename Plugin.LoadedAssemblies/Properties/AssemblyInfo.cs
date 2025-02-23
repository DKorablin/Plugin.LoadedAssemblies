using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("8952337f-4c04-469e-908a-bf9fa1d3bdbd")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.LoadedAssemblies")]
#else

[assembly: AssemblyDescription("View loaded assemblies and libraries in the current process")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2025")]
#endif