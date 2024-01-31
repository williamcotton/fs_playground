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
            Html.h1 "Fable Universal Express Demo"
            Html.div [
                contentElement
            ]
    ]])

[<ReactComponent>]
let Counter() =
    let (count, setCount) = React.useState(0)
    let req : ExpressReq = React.useContext(requestContext)
    Html.div [
        Html.h2 count
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
        Html.h2 [
            prop.text "Form POST Demo"
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

let verifyPost value =
    match value with
    | Some(value) -> 
        match value with
        "test" -> 
            Some("test")
        | _ ->
            None
    | None -> 
        None

let errorHandler (err: obj) (req: ExpressReq) (res: ExpressRes) (next: unit -> unit) =
    match err with
    | :? System.Exception as ex ->
        consoleLog ex.Message
        res.send (sprintf "An error occurred: %s" ex.Message)
    | _ ->
        next()

let notFoundHandler (req: ExpressReq) (res: ExpressRes) (next: unit -> unit) =
    res.status 404 |> ignore
    res.renderComponent(Html.p "Not found") |> ignore

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
        let test = req.``body``?test :> string option
        match verifyPost test with
        | Some(value) -> 
            res.renderComponent(Html.p ("Value: " + value) )|> ignore
        | None -> 
            res.renderComponent(Html.p "Not valid") |> ignore
    )

    app.``use`` notFoundHandler
    app.``use`` errorHandler