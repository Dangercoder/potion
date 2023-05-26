using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using clojure.lang;

namespace Program
{
    public static class CljMain
    {
        private static readonly Symbol CLOJURE_MAIN = Symbol.intern("clojure.main");
        private static readonly Var REQUIRE = RT.var("clojure.core", "require");
        private static readonly Var LEGACY_REPL = RT.var("clojure.main", "legacy-repl");
        private static readonly Var LEGACY_SCRIPT = RT.var("clojure.main", "legacy-script");
        private static readonly Var MAIN = RT.var("clojure.main", "main");
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
                try {
                Console.WriteLine(originalTargetFramework);
                var targetPackages = projectAssetsJson.GetProperty("targets").GetProperty(originalTargetFramework.ToString());

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

                } catch (Exception e) {
                    
                }
            }}

            static void Main(string[] args)
            {

                var x = 32.GetType().Assembly.Location;
                Console.WriteLine(x);


                var coreAppAssemblyDirectory = Path.GetDirectoryName(x);
                var aspNetCoreAppAssemblyDirectory = coreAppAssemblyDirectory.Replace("Microsoft.NETCore.App", "Microsoft.AspNetCore.App");

                LoadAllAssembliesInDirectory(coreAppAssemblyDirectory);
                LoadAllAssembliesInDirectory(aspNetCoreAppAssemblyDirectory);
                LoadNugetPackages();

                RT.Init();
                REQUIRE.invoke(CLOJURE_MAIN);
                MAIN.applyTo(RT.seq(args));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "ClojureJVM name match")]
            public static void legacy_repl(string[] args)
            {
                RT.Init();
                REQUIRE.invoke(CLOJURE_MAIN);
                LEGACY_REPL.invoke(RT.seq(args));

            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "ClojureJVM name match")]
            public static void legacy_script(string[] args)
            {
                RT.Init();
                REQUIRE.invoke(CLOJURE_MAIN);
                LEGACY_SCRIPT.invoke(RT.seq(args));
            }


        }
    }


// See https://aka.ms/new-console-template for more information