﻿module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express
open Fable.Core.JsInterop
open Express

let requestContext = React.createContext(name="Request")

[<ReactComponent>]
let AppLayout params =
    let contentElement : ReactElement =
        emitJsExpr () "params.content"
    let req : ExpressReq = 
        emitJsExpr () "params.req"
    React.contextProvider(requestContext, req, React.fragment [
        Html.div [
            Html.h1 "Hello World"
            Html.div [
                contentElement
            ]
    ]])

[<ReactComponent>]
let Counter() =
    let (count, setCount) = React.useState(0)
    let req : ExpressReq = React.useContext(requestContext)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
        req.Link {| href = "/test"; children = "Test" |} 
    ]

[<ReactComponent>]
let Test() =
    let (count, setCount) = React.useState(0)
    let req : ExpressReq = React.useContext(requestContext)
    Html.div [
        Html.h1 [
            prop.text "Test"
        ]
        req.Form {| action = "/test_post"; method = "POST"; children = [
            Html.input [
                prop.type' "text"
                prop.name "test"
            ]
            Html.input [
                prop.type' "submit"
                prop.value "Submit"
            ]
        ] |}
    ]
    

let universalApp (app: ExpressApp) =
    app.get("/", fun req res _ ->
        res.renderComponent(Counter())
        |> ignore
    )

    app.get("/test", fun req res _ ->
        res.renderComponent(Test())
        |> ignore
    )
    
    app.post("/test_post", fun req res _ ->
        let body = req.body
        let test = req.``body``?test
        match test with
        | Some(value) -> 
            res.send(value) |> ignore
        | None -> 
            res.send("No name provided") |> ignore
    )
