# Potion

A [clojure-clr](https://github.com/clojure/clojure-clr) tool for .NET 3.1+

## Installation
`dotnet tool install --global potion`

## Commands
| Command | Description |
| ------- | ----------- |
| `potion` | Starts a clojure repl with all assemblies loaded from the `.csproj` file |
| `potion repl` | Starts a nrepl server on `localhost:6667` with all assemblies loaded from the `.csproj` file |
| `potion test` | Run tests located in the `test` directory |

### Calva Setup
1. run `potion repl`
2. Calva: Connect to a Running Repl Server -> Generic -> `localhost:6667`