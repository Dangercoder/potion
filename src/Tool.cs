using System;
using clojure.lang;

namespace Tool
{
    public static class Runner
    {
        private static readonly Symbol CLOJURE_MAIN = Symbol.intern("clojure.main");
        private static readonly Var REQUIRE = RT.var("clojure.core", "require");
        private static readonly Var LEGACY_REPL = RT.var("clojure.main", "legacy-repl");
        private static readonly Var LEGACY_SCRIPT = RT.var("clojure.main", "legacy-script");
        private static readonly Var MAIN = RT.var("clojure.main", "main");

        static void Main(string[] args)
        {
            DependencyLoader.Load();

            if (args.Length > 0 && args[0] == "test")
            {
                TestRunner.Run();
                return;
            }

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