# Potion
A [clojure-clr](https://github.com/clojure/clojure-clr) tool that loads assemblies from project.csproj
and launches clojure.

Requires .NET 6 or higher

## Installation
`dotnet tool install --global potion`


# Usage
* Be in the same directory as your .csproj  
* `dotnet restore`
* `potion` - This starts a clojure repl with all assemblies loaded from the `.csproj` file  
 
