using System.Collections.Generic;
using System.IO;
using clojure.lang;

namespace Tool
{
    public static class TestRunner
    {
        private static readonly Var REQUIRE = RT.var("clojure.core", "require");
        private static readonly Var LOAD = RT.var("clojure.core", "load");

        public static void Run(string testRoot = "test", string srcRoot = "src")
        {
            

            var directories = new List<string> { srcRoot, testRoot };

            foreach (var directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    var cljFiles = Directory.GetFiles(directory, "*.cljr", SearchOption.AllDirectories);

                    foreach (var file in cljFiles)
                    {
                        var loadPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
                            .Replace('\\', '/') // Convert backslashes to slashes (for Windows paths)
                            .Substring(0, file.Length - ".cljr".Length); // Remove the file extension.

                        // Convert the file path to a Clojure namespace.
                        var ns = loadPath
                            .Replace('/', '.')
                            .Replace("_", "-"); // Convert slashes to dots.

                        // Then require the namespace.
                        REQUIRE.invoke(Symbol.intern(ns));

                        // ns will start with .test -- remove it so that run-tests works.
                        if (ns.StartsWith("test."))
                        {
                            ns = ns.Substring("test.".Length);
                            // TODO system exit.
                            var testRunner = RT.var("clojure.test", "run-tests");
                            testRunner.invoke(Symbol.intern(ns));
                        }
                    }
                }
            }
        }
    }
}
