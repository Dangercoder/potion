using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Tool
{
    public static class DependencyLoader
    {
        static void LoadAllAssembliesInDirectory(string directoryPath)
        {
            Console.WriteLine($"Loading assemblies in: {directoryPath}");

            foreach (var filePath in System.IO.Directory.GetFiles(directoryPath))
            {
                if (System.IO.Path.GetExtension(filePath) == ".dll")
                {
                    try
                    {
                        Assembly.LoadFrom(filePath);
                    }
                    catch (Exception)
                    {
                        // Ignoring exceptions
                    }
                }
            }
        }

        public static void Load()
        {
            var x = 32.GetType().Assembly.Location;
            Console.WriteLine(x);


            var coreAppAssemblyDirectory = Path.GetDirectoryName(x);
            var aspNetCoreAppAssemblyDirectory = coreAppAssemblyDirectory.Replace("Microsoft.NETCore.App", "Microsoft.AspNetCore.App");
            var winDesktopAppAssemblyDirectory = coreAppAssemblyDirectory.Replace("Microsoft.NETCore.App", "Microsoft.WindowsDesktop.App");

            LoadAllAssembliesInDirectory(coreAppAssemblyDirectory);
            LoadAllAssembliesInDirectory(aspNetCoreAppAssemblyDirectory);
            LoadAllAssembliesInDirectory(winDesktopAppAssemblyDirectory);
            DependencyLoaderCsProj.LoadDependenciesFromCsproj();
        }

    }

}