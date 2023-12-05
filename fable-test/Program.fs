module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express

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

[<ReactComponent>]
let Counter() =
    let (count, setCount) = React.useState(0)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]

open Browser.Dom

// let root = ReactDOM.createRoot(document.getElementById "root")
// root.render(Counter())

let d = ReactDOMServer.renderToString(Counter())

printfn "%s" d

app.get("/", fun req res _ ->
    res.send(d)
    |> ignore
)

app.listen(3000, fun _ ->
    printfn "Listening on port 3000"
)
