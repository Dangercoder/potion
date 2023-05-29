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

        static void LoadNugetPackages()
        {
            var projectAssetsPath = "obj/project.assets.json";
            var jsonDocumentOptions = new JsonDocumentOptions();
            var jsonSerializerOptions = new JsonSerializerOptions();

            using var streamReader = new StreamReader(projectAssetsPath);
            var jsonString = streamReader.ReadToEnd();

            using var jsonDocument = JsonDocument.Parse(jsonString, jsonDocumentOptions);

            var projectAssetsJson = jsonDocument.RootElement;
            var packagesPath = projectAssetsJson.GetProperty("project").GetProperty("restore").GetProperty("packagesPath").GetString();
            var originalTargetFrameworks = projectAssetsJson.GetProperty("project").GetProperty("restore").GetProperty("originalTargetFrameworks").EnumerateArray();

            foreach (var originalTargetFramework in originalTargetFrameworks)
            {
                try
                {
                    Console.WriteLine(originalTargetFramework);
                    var targetPackages = projectAssetsJson.GetProperty("targets").GetProperty(originalTargetFramework.ToString());

                    // TODO this loads too much if there's multiple targets.
                    foreach (var targetPackage in targetPackages.EnumerateObject())
                    {
                        Console.WriteLine(targetPackage.Name);
                        var dllPath = $"{packagesPath}/{targetPackage.Name.ToLower()}/{targetPackage.Value.GetProperty("runtime").EnumerateObject().FirstOrDefault().Name}";

                        if (dllPath.Contains(".dll"))
                        {
                            try
                            {
                                Assembly.LoadFrom(dllPath);
                            }
                            catch (Exception)
                            {
                                // Ignoring exceptions
                            }
                        }
                    }

                }
                catch (Exception e)
                {

                }
            }
        }
        public static void Load()
        {
            var x = 32.GetType().Assembly.Location;
            Console.WriteLine(x);


            var coreAppAssemblyDirectory = Path.GetDirectoryName(x);
            var aspNetCoreAppAssemblyDirectory = coreAppAssemblyDirectory.Replace("Microsoft.NETCore.App", "Microsoft.AspNetCore.App");

            LoadAllAssembliesInDirectory(coreAppAssemblyDirectory);
            LoadAllAssembliesInDirectory(aspNetCoreAppAssemblyDirectory);
            LoadNugetPackages();
        }

    }

}