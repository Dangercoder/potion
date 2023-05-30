## dotnet minimal api
Showcases how to use clojure-clr with .NET 7's minimal api.

## Usage
connect to a repl and eval the whole file and (start) (stop) in the comment block.
   
### endpoints
localhost:4999/order -- POST -- any json body. 
localhost:4999/      -- GET  -- returns a generated response body based on clojure.spec definitions

### I hope that we at one point can get to this state:

`C#`
```C#
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());
```

`Clojure-Clr`
```Clojure
(.MapGet app "/todoitems", ^:async (fn [^TodoDb db] (await (-> db .-Todos (.ToListAsync)))))
```
  