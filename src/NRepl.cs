using System.Collections.Generic;
using System.IO;
using clojure.lang;

namespace Tool
{
    public static class NreplRunner
    {
        private static readonly Var REQUIRE = RT.var("clojure.core", "require");
        private static readonly Var LOAD = RT.var("clojure.core", "load");

        public static void Run()
        {
            var ns = "potion.nrepl";
            LOAD.invoke("/src/potion/nrepl");
            REQUIRE.invoke(Symbol.intern(ns));
            var fn = RT.var(ns, "start-nrepl");
            fn.invoke();
        }
    }
}
