namespace Tool
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using System.IO;
    using System.Collections.Generic;

    public static class DependencyLoaderCsProj
{
    static List<string> Frameworks;
    static HashSet<string> loadedAssemblyPaths = new HashSet<string>();

    public static void LoadDependenciesFromCsproj()
    {
        var csprojFilePath = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

        if (csprojFilePath == null)
        {
            throw new FileNotFoundException("No .csproj file found in the current directory.");
        }

        XDocument csprojXml = XDocument.Load(csprojFilePath);

        // .csproj files use namespaces, we need to get it to use in our queries
        XNamespace ns = csprojXml.Root.Name.Namespace;

        // Set the target frameworks
        Frameworks = new List<string>();

        // If it's a multi-targeting project
        var targetFrameworksProperty = csprojXml.Descendants(ns + "TargetFrameworks").FirstOrDefault();
        if (targetFrameworksProperty != null)
        {
            var targetFrameworks = targetFrameworksProperty.Value.Split(';').ToList();
            Frameworks.Add(targetFrameworks.Last());
        }
        else
        {
            // If it's a single-targeting project
            var targetFrameworkProperty = csprojXml.Descendants(ns + "TargetFramework").FirstOrDefault();
            if (targetFrameworkProperty != null)
            {
                Frameworks.Add(targetFrameworkProperty.Value);
            }
        }

        // Select all PackageReference elements
        var packageReferences = csprojXml.Descendants(ns + "PackageReference").ToList();

        foreach (var packageReference in packageReferences)
        {
            // Get the ID of the package
            var id = (string)packageReference.Attribute("Include");
            // Get the version of the package
            var version = (string)packageReference.Attribute("Version");

            // The path to the .nuspec file of the package
            var nuspecFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", id.ToLower(), version, $"{id.ToLower()}.nuspec");

            // Load dependencies from the .nuspec file
            LoadDependenciesFromNuspec(nuspecFilePath);

            // Load the assembly of the package
            LoadAssemblyForPackage(id, version);
        }
    }

    public static void LoadDependenciesFromNuspec(string nuspecFilePath)
    {
        XDocument nuspecXml = XDocument.Load(nuspecFilePath);

        // .nuspec files use namespaces, we need to get it to use in our queries
        XNamespace ns = nuspecXml.Root.GetDefaultNamespace();

        // Select all dependency elements
        var dependencies = nuspecXml.Descendants(ns + "dependencies")
                                    .Elements(ns + "group")
                                    .Elements(ns + "dependency")
                                    .ToList();

        foreach (var dependency in dependencies)
        {
            // Get the ID of the dependency
            var id = (string)dependency.Attribute("id");
            // Get the version of the dependency
            var version = (string)dependency.Attribute("version");

            // Load the assembly of the dependency
            LoadAssemblyForPackage(id, version);
        }
    }

    private static void LoadAssemblyForPackage(string packageId, string version)
{
    // Define all available frameworks in descending order of compatibility
    string[] allFrameworks = { "net7.0", "net6.0", "net5.0", "netcoreapp3.1", "netstandard2.1" };

    // Determine the starting index in the allFrameworks array based on the target framework
    int startIndex = Array.IndexOf(allFrameworks, Frameworks.First());

    // Loop through the frameworks starting from the target framework
    for (int i = startIndex; i < allFrameworks.Length; i++)
    {
        var assemblyFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", packageId.ToLower(), version, "lib", allFrameworks[i], $"{packageId}.dll");

        if (File.Exists(assemblyFilePath))
        {
            try
            {
                  if (loadedAssemblyPaths.Contains(assemblyFilePath))
                    {
                        return;
                    }

                var assembly = Assembly.LoadFrom(assemblyFilePath);
                loadedAssemblyPaths.Add(assemblyFilePath);
                Console.WriteLine(assembly);
                return;  // Once the assembly is loaded, we don't need to continue the loop
            }
            catch (Exception e)
            {
                // This exception is thrown when the file is not a valid assembly, or when the assembly is not compatible
                // In this case, we simply ignore the exception and continue to the next framework
            }
        }
    }
}



}

}