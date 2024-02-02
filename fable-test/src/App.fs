module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Express

let requestContext = React.createContext(name="Request")

[<ReactComponent>]
let AppLayout (props: {| content: ReactElement; req: ExpressReq |}) =
    React.contextProvider(requestContext, props.req, React.fragment [
        Html.div [
            prop.className "body" |> ignore
            yield props.req.Link {| href = "/"; children = [ Html.h1 "Fable Universal Express Demo" ] |} 
            yield Html.div [
                props.content
            ]
        ]
    ])

[<ReactComponent>]
let Counter() =
    let (count, setCount) = React.useState(0)
    let req : ExpressReq = React.useContext(requestContext)
    React.fragment [
        Html.h2 count
        Html.button [
            prop.text "Increment"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
        req.Link {| href = "/test"; children = "Test" |} 
        req.Link {| href = "/error"; children = "Error" |} 
    ]

[<ReactComponent>]
let Button(props: {| title: string |}) =
    Html.input [
        prop.type' "submit"
        prop.value props.title
    ]

[<ReactComponent>]
let Test() =
    let req : ExpressReq = React.useContext(requestContext)
    React.fragment [
        Html.h2 [
            prop.text "Form POST Demo"
        ]
        req.Form {| action = "/test_post"; method = "POST"; children = [
            Html.input [
                prop.type' "text"
                prop.name "test"
            ]
            Button {| title = "Submit" |}
        ] |}
    ]

let verifyPost (value: string option) =
    match value with
    | Some s when s.Length >= 5 -> Ok s
    | Some _ -> Error "The value must be at least 6 characters long."
    | None -> Error "No value provided."

let errorHandler (err: obj) (req: ExpressReq) (res: ExpressRes) (next: unit -> unit) =
    match err with
    | :? System.Exception as ex ->
        consoleLog ex.Message
        let message = ex.Message
        res.status 500 |> ignore
        res.send(message) |> ignore
    | _ ->
        next()

let universalApp (app: ExpressApp) =
    app.get("/", fun req res _ ->
        res.renderComponent(Counter())
        |> ignore
    )

    app.get("/test", fun req res _ ->
        res.renderComponent(Test())
        |> ignore
    )

    app.get("/error", fun req res _ -> raise (System.Exception("Some sort of detailed error message")))
    
    app.post("/test_post", fun req res _ ->
        let body = req.body
        let test: string option = req.``body``?test
        match verifyPost test with
        | Ok(value) ->
            res.renderComponent(Html.p ("Value: " + value)) |> ignore
        | Error(msg) ->
            res.status 400 |> ignore
            res.renderComponent(Html.p msg) |> ignore
    )

    app.``use`` (fun (req: ExpressReq) (res: ExpressRes) next ->
        res.status 404 |> ignore
        res.renderComponent(Html.p "Page not found") |> ignore
    )

    app.``use`` errorHandler
