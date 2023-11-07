#!/usr/bin/env dotnet fsi

let safeDivide y x = 
  if y = 0.0 then None
  else Some (x / y)

let result = 
  Some 10.0
  |> Option.bind (safeDivide 5.0)
  |> Option.bind (safeDivide 2.0)

match result with
| Some x -> printfn "Result: %f" x
| None -> printfn "Cannot divide by zero"