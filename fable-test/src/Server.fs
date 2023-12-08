module Server

open Node.Api
open Feliz
open Fable.Core
open App
open Express

let sessionSecret = "tempsecret"

[<Import("default", "express")>]
let express : unit -> ExpressApp = jsNative

[<Import("default", "csurf")>]
let csurf : unit -> unit = jsNative

[<Import("default", "cookie-session")>]
let cookieSession : {| name: string; sameSite: string; secret: string |} -> unit = jsNative

[<Import("default", "./server/middleware/express-link.js")>]
let expressLinkMiddleware : {| title: string |} -> unit = jsNative

[<Import("default", "./server/middleware/react-renderer.js")>]
let reactRendererMiddleware : {| appLayout: (unit -> ReactElement) -> ReactElement |} -> unit = jsNative

[<Emit("app.use($0)")>]
let useMiddleware middleware: unit = jsNative

[<Emit("express.static($0)")>]
let expressStatic (path : string): unit = jsNative

let app = express()
useMiddleware(expressStatic("public"))
useMiddleware(cookieSession({| name = "session"; sameSite = "lax"; secret = sessionSecret |}))
useMiddleware(csurf())
useMiddleware(expressLinkMiddleware({| title = "Test"|}))
useMiddleware(reactRendererMiddleware({| appLayout = AppLayout |}))

baseRoute app 

app.listen(3000, fun _ ->
    printfn "Listening on port 3000"
)