#!/usr/bin/env dotnet fsi

type Result<'T> = 
  | Success of 'T
  | Error of string

let safeDivide y x = 
  if y = 0.0 then Error "Cannot divide by zero"
  else Success (x / y)  

type ResultBuilder() =
  member this.Bind(x, f) =
      match x with
      | Success a -> f a
      | Error e -> Error e
  member this.Return(x) = Success x
  member this.ReturnFrom(x) = x

let result = ResultBuilder()

let computation= result {
  let! a = Success 10.0
  let! b = safeDivide 5.0 a
  let! c = safeDivide 2.0 b
  return c
}

match computation with
| Success x -> printfn "Result: %f" x
| Error e -> printfn "Error: %s" e