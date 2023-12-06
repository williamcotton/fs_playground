module Server

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express
open App

type ExpressReq =
    abstract member params : obj
  
type ExpressRes =
    abstract member send : obj -> unit

type ExpressApp =
    abstract member get: string * (ExpressReq -> ExpressRes -> unit -> unit) -> unit
    abstract member listen: int * (unit -> unit) -> unit

[<Import("default", "express")>]
let express : unit -> ExpressApp = jsNative
let app = express()

let d = ReactDOMServer.renderToString(Counter())

printfn "%s" d

app.get("/", fun req res _ ->
    res.send(d)
    |> ignore
)

app.listen(3000, fun _ ->
    printfn "Listening on port 3000"
)