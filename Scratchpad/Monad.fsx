#!/usr/bin/env dotnet fsi

// Type Constructor
// In category theory, this is known as an endofunctor. It's a functor within a single category.
// In this case, the category is the category of types in F#, and Result<'T> is a functor that maps a type 'T to a new type Result<'T>.
type Result<'T> = 
  | Success of 'T
  | Error of string

// Unit Function
// In category theory, this is part of the definition of a monad. It's also known as the 'return' operation in monadic context.
// It takes a value and puts it into a context, in this case, the Result context.
let unit (value: 'T) : Result<'T> = Success value

// Bind Function
// This is another part of the definition of a monad, often called 'bind' or '>>='.
// It takes a context (Result) and a function that goes from the underlying type to another context, and connects them together.
// If the context is a Success, it applies the function to the value inside. If it's an Error, it propagates the error.
let bind f res = 
  match res with
  | Success x -> f x
  | Error e -> Error e

// Join Function
// This is also part of the definition of a monad. It's used to flatten a nested context.
// In this case, it takes a Result of a Result (Result<Result<'T>>) and combines them into a single Result.
// If the outer Result is a Success, it returns the inner Result. If the outer Result is an Error, it propagates the error.
let join res = 
  match res with
  | Success (Success x) -> Success x
  | Success (Error e) -> Error e
  | Error e -> Error e

// Map Function
// This is a function that takes a function from the underlying type to another type, and applies it to the value inside the context.
// In this case, it takes a function from 'T to 'U, and a Result<'T>, and returns a Result<'U>.
let map f res = 
  match res with
  | Success x -> Success (f x)
  | Error e -> Error e

// A function that might fail, returning a Result.
// This is an example of a function that you might use with the bind function to chain together computations that might fail.
let safeDivide y x = 
  if y = 0.0 then Error "Cannot divide by zero"
  else Success (x / y)

// A computation using the Result monad.
// The bind function is used to chain together the computations, handling errors in a consistent way.
let result = 
  Success 10.0
  |> bind (safeDivide 5.0)
  |> bind (safeDivide 2.0)

// The result of the computation is handled.
// If it's a Success, the value is printed. If it's an Error, the error message is printed.
match result with
| Success x -> printfn "Result: %f" x
| Error e -> printfn "Error: %s" e

// An example of using the join function.
// Here, we have a Result of a Result (a nested context), and we use join to flatten it into a single Result.
let nestedResult = Success (Success 10.0)
let flattenedResult = join nestedResult

// In category theory, join is part of the definition of a monad. It's used to flatten a nested monadic value.
// In this case, it takes a Result of a Result (Result<Result<'T>>) and combines them into a single Result.
// If the outer Result is a Success, it returns the inner Result. If the outer Result is an Error, it propagates the error.
match flattenedResult with
| Success x -> printfn "Flattened Result: %f" x
| Error e -> printfn "Error: %s" e

// An example of using the map function.
// Here, we have a function that takes a value and returns a Result, and we use map to apply it to a Result.
let safeDivideByTwo = safeDivide 2.0
let mappedResult = 
    match join (map safeDivideByTwo (Success 10.0)) with 
    | Success x -> x 
    | Error e -> 0.0