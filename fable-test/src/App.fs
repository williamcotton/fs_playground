module App

open Node.Api
open Feliz
open Fable.Core
open Fable.Import.Express
open Fable.Core.JsInterop
open Express

let requestContext = React.createContext(name="Request")

type AppLayoutParams = {
    content: ReactElement
    req: ExpressReq
}

[<ReactComponent>]
let AppLayout (params: AppLayoutParams) =
    React.contextProvider(requestContext, params.req, React.fragment [
        Html.div [
            prop.className "body"
            yield params.req.Link {| href = "/"; children = [ Html.h1 "Fable Universal Express Demo" ] |} 
            yield Html.div [
                params.content
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

type ButtonProps = {
    title: string
}

[<ReactComponent>]
let Button(props: ButtonProps) =
    Html.input [
        prop.type' "submit"
        prop.value props.title
    ]

[<ReactComponent>]
let Test() =
    let (count, setCount) = React.useState(0)
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
            Button { title = "Submit" }
        ] |}
    ]

// [<JSX.Component>]
// let JSXTest() = // (Msg -> unit) -> JSX. Element
//     JSX.jsx
//         $"""
//         <div>
//             <h4>JSX!</h4>
//         </div>
//         """

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
        let message = ex.Message
        res.status 500 |> ignore
        res.renderComponent(Html.div [ Html.p "We're sorry, something went wrong!"; Html.pre message]) |> ignore
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

    // app.get("/jsx", fun req res _ ->
    //     res.renderComponent(JSXTest())
    //     |> ignore
    // )

    app.get("/error", fun req res _ -> raise (System.Exception("Some sort of detailed error message")))
    
    app.post("/test_post", fun req res _ ->
        let body = req.body
        let test = req.``body``?test :> string option
        match verifyPost test with
        | Some(value) -> 
            res.renderComponent(Html.p ("Value: " + value) )|> ignore
        | None -> 
            res.renderComponent(Html.p "Not valid") |> ignore
    )

    app.``use`` (fun (req: ExpressReq) (res: ExpressRes) next ->
        res.status 404 |> ignore
        res.renderComponent(Html.p "Page not found") |> ignore
    )

    app.``use`` errorHandler
