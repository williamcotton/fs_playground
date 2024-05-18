type Result<'T> =
    | Success of 'T
    | Error of string

type LogState = { Logs: string list }

type ResultLog<'T> = Result<'T> * LogState

type ResultBuilder() =
    member _.Bind(x: ResultLog<'T>, f) = 
        match x with
        | Success v, (state: LogState) -> 
            let result, newState = f v
            match result with
            | Success _ ->
              result, { state with Logs = state.Logs @ newState.Logs }
            | Error e -> (Error e, { state with Logs = state.Logs @ newState.Logs @ [sprintf "Error: %s" e] })
        | Error e, state ->
            Error e, state
            
    member _.Return(x) = 
        Success x, { Logs = [] }
    
    member _.Zero(x) = 
        Success x, { Logs = [] }

    member _.Yield(x) = 
        Success x, { Logs = [] }

    member _.For(x: ResultLog<'T>, f) = 
        match x with
        | Success v, (state: LogState) -> 
            let (result, newState) = f v
            match result with
            | Success _ ->
              result, { state with Logs = state.Logs @ newState.Logs }
            | Error e -> (Error e, { state with Logs = state.Logs @ newState.Logs @ [sprintf "Error: %s" e] })
        | Error e, state ->
            Error e, state

    member _.Run x = x

    [<CustomOperation("log")>]
    member _.Log (x, message: string): ResultLog<'T> = 
        match x with
        | Success v, (state: LogState) -> 
            Success v, { state with Logs = state.Logs @ [message] }
        | Error e, state ->
            Error e, state


let result = ResultBuilder()

let multiply10 x = 
    if x > 0 then Success (x * 10), { Logs = [sprintf "multiply10 successful: %d" (x * 10)] }
    else Error "x must be positive", { Logs = ["multiply10 failed: x must be positive"] }
    
let add1 x = 
    if x < 100 then Success (x + 1), { Logs = [sprintf "add1 successful: %d" (x + 1)] }
    else Error "result too large", { Logs = ["add1 failed: x too large"] }

let num1 = result {
    let! a = multiply10 5
    let! b = add1 a
    return b
}

match num1 with
| Success finalResult, logs -> 
    printfn "Final result: %d" finalResult
    printfn "Logs: %A" logs.Logs
| Error e, logs -> 
    printfn "An error occurred: %s" e
    printfn "Logs: %A" logs.Logs

let num2 = result {
    log "starting calculation" // no error, properly sets the first message of the Logs
    let! a = multiply10 50
    //log "starting calculation" // This expression was expected to have type 'int' but here has type 'unit'  
    let! b = add1 a
    let! c = multiply10 b
    return c
}

match num2 with
| Success finalResult, logs -> 
    printfn "Final result: %d" finalResult
    printfn "Logs: %A" logs.Logs
| Error e, logs ->
    printfn "An error occurred: %s" e
    printfn "Logs: %A" logs.Logs
