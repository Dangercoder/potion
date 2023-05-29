# Potion

A [clojure-clr](https://github.com/clojure/clojure-clr) tool for .NET 3.1+

## Installation
`dotnet tool install --global potion`

`dotnet add package clojure.tools.nrepl --version 0.1.0-alpha1` is needed
because .NET tools does not include dependencies.

## Commands
| Command | Description |
| ------- | ----------- |
| `potion` | Starts a clojure repl with all assemblies loaded from the `.csproj` file |
| `potion repl` | Starts a nrepl server on `localhost:1667` with all assemblies loaded from the `.csproj` file |
| `potion test` | Run tests located in the `test` directory |

### Calva Setup
1. run `potion repl`
2. Calva: Connect to a Running Repl Server -> Generic -> `localhost:1667`


### Creating a new project

```bash
 mkdir -p minimal-example && cd minimal-example && dotnet new console --framework net7.0 && dotnet add package clojure.tools.nrepl --version 0.1.0-alpha1 && rm Program.cs
```