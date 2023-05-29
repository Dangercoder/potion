# Potion

A [clojure-clr](https://github.com/clojure/clojure-clr) tool for .NET 3.1+

## Installation
`dotnet tool install --global potion`

## Commands
| Command | Description |
| ------- | ----------- |
| `potion` | Starts a clojure repl with all assemblies loaded from the `.csproj` file |
| `potion test` | Run tests located in the `test` directory |

### Example usage
* Be in the same directory as your .csproj  
* `dotnet restore`
* `potion` - starts a clojure repl
* `potion test` - runs all tests under `/test`
