using System;
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

            Console.WriteLine("Loading NREPL");
            // FIXME use potion.nrepl to start the server.
//            System.Environment.SetEnvironmentVariable("CLOJURE_LOAD_PATH", "src");
            //  var ns = "potion.nrepl";
            //  REQUIRE.invoke(Symbol.intern(ns));
            //  var fn = RT.var(ns, "start-nrepl");
            //  fn.invoke();
            
             RT.Init();

             var ns = "clojure.tools.nrepl";
             REQUIRE.invoke(Symbol.intern(ns));
             var fn = RT.var(ns, "start-server!");
             fn.invoke();
        }
    }
}
