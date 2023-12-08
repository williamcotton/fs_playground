module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express
open Fable.Core.JsInterop
open Express

[<ReactComponent>]
let AppLayout params =
    let contentElement : ReactElement =
        emitJsExpr () "params.content"
    Html.div [
        Html.h1 "Hello World"
        Html.div [
            contentElement
        ]
    ]

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

let baseRoute (app: ExpressApp) =
    app.get("/", fun req res _ ->
        res.renderComponent(Counter())
        |> ignore
    )
