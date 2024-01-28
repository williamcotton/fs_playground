module Express.Server

open Node.Api
open Feliz
open Fable.Core
open App
open Express

[<Import("default", "express")>]
let express : unit -> ExpressApp = jsNative

[<Import("default", "csurf")>]
let csurf : unit -> unit = jsNative

[<Import("default", "cookie-session")>]
let cookieSession : {| name: string; sameSite: string; secret: string |} -> unit = jsNative

[<Import("default", "./middleware/express-link.js")>]
let expressLinkMiddleware : {| title: string |} -> unit = jsNative

[<Import("default", "./middleware/react-renderer.js")>]
let reactRendererMiddleware : {| appLayout: obj -> ReactElement |} -> unit = jsNative

[<Emit("app.use($0)")>]
let useMiddleware middleware: unit = jsNative

[<Emit("express.static($0)")>]
let expressStatic (path : string): unit = jsNative