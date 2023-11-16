type ParseResult<'T> =
  | Ok of 'T
  | Error of string

type Parser<'T> = Parser of (string -> ParseResult<'T * string>)

let run parser input =
  let (Parser innerFn) = parser
  innerFn input

let andThen parser1 parser2 =
  let innerFn input =
    // run parser1 with the input
    let result1 = run parser1 input

    // test the result for Failure/Success
    match result1 with
    | Error err -> Error err // return error from parser1
    | Ok (value1,remaining1) ->
      // run parser2 with the remaining input
      let result2 =  run parser2 remaining1

      // test the result for Failure/Success
      match result2 with
      | Error err -> Error err // return error from parser2
      | Ok (value2,remaining2) ->
        // combine both values as a pair
        let newValue = (value1,value2)
        // return remaining input after parser2
        Ok (newValue,remaining2)

  // return the inner function
  Parser innerFn

let ( .>>. ) = andThen

let orElse parser1 parser2 =
  let innerFn input =
    // run parser1 with the input
    let result1 = run parser1 input

    // test the result for Failure/Success
    match result1 with
    | Ok result -> result1 // if success, return the original result
    | Error err ->
      // if failed, run parser2 with the input
      let result2 = run parser2 input

      // return parser2's result
      result2

  // return the inner function
  Parser innerFn

let ( <|> ) = orElse

let choice listOfParsers =
  List.reduce ( <|> ) listOfParsers

let pchar charToMatch =
  let innerFn str =
    if System.String.IsNullOrEmpty(str) then
      let msg = "No more input"
      Error msg
    else if str.[0] = charToMatch then
      let remaining = str.[1..]
      Ok (charToMatch, remaining)
    else
      let msg = sprintf "Expected %c but got %c" charToMatch str.[0]
      Error msg
  Parser innerFn

let parseA = pchar 'a'
let parseB = pchar 'b'
run parseA "abc"

let parseAThenB = parseA .>>. parseB
run parseAThenB "abc"

let parseAOrElseB = parseA <|> parseB
run parseAOrElseB "abc"

