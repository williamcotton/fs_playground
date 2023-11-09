// Type Constructor
// In category theory, this is known as an endofunctor. It's a functor within a single category.
// In this case, the category is the category of types in TypeScript, and Result<T> is a functor that maps a type T to a new type Result<T>.
type Result<T> = 
  | { kind: "success", value: T }
  | { kind: "error", error: string }

// Unit Function
// In category theory, this is part of the definition of a monad. It's also known as the 'return' operation in monadic context.
// It takes a value and puts it into a context, in this case, the Result context.
function unit<T>(value: T): Result<T> {
  return { kind: "success", value };
}

// Bind Function
// This is another part of the definition of a monad, often called 'bind' or '>>='.
// It takes a context (Result) and a function that goes from the underlying type to another context, and connects them together.
// If the context is a Success, it applies the function to the value inside. If it's an Error, it propagates the error.
function bind<T, U>(f: (value: T) => Result<U>, res: Result<T>): Result<U> {
  if (res.kind === "success") {
    return f(res.value);
  } else {
    return res;
  }
}

// Join Function
// This is also part of the definition of a monad. It's used to flatten a nested context.
// In this case, it takes a Result of a Result (Result<Result<T>>) and combines them into a single Result.
// If the outer Result is a Success, it returns the inner Result. If the outer Result is an Error, it propagates the error.
function join<T>(res: Result<Result<T>>): Result<T> {
  if (res.kind === "success") {
    return res.value;
  } else {
    return res;
  }
}

// A function that might fail, returning a Result.
// This is an example of a function that you might use with the bind function to chain together computations that might fail.
function safeDivide(y: number, x: number): Result<number> {
  if (y === 0.0) {
    return { kind: "error", error: "Cannot divide by zero" };
  } else {
    return { kind: "success", value: x / y };
  }
}

// A computation using the Result monad.
// The bind function is used to chain together the computations, handling errors in a consistent way.
let result = 
  bind((x: number) => safeDivide(5.0, x),
  bind((x: number) => safeDivide(2.0, x),
  unit(10.0)));

// The result of the computation is handled.
if (result.kind === "success") {
  console.log(`Result: ${result.value}`);
} else {
  console.log(`Error: ${result.error}`);
}

// An example of using the join function.
let nestedResult: Result<Result<number>> = unit(unit(10.0));
let flattenedResult = join(nestedResult);

// The result of the join operation is handled.
if (flattenedResult.kind === "success") {
  console.log(`Flattened Result: ${flattenedResult.value}`);
} else {
  console.log(`Error: ${flattenedResult.error}`);
}