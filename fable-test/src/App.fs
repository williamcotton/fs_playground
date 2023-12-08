module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express
open Fable.Core.JsInterop
open Express

[<ReactComponent>]
let AppLayout (content: string, req: ExpressReq) =
    printfn "%s" content
    let contentElement : ReactElement =
        emitJsExpr () "appLayoutInputProps.params.content"
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
            prop.text "Increment123"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]

let baseRoute (app: ExpressApp) =
    app.get("/", fun req res _ ->
        res.renderComponent(Counter())
        |> ignore
    )
