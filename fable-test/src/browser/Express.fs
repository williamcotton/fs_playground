module Express.Browser

open Browser.Dom
open Feliz
open Fable.Core
open App

[<Import("default", "browser-express")>]
let express : unit -> ExpressApp = jsNative

[<Import("default", "./middleware/express-link.js")>]
let expressLinkMiddleware : unit -> unit = jsNative

[<Import("default", "./middleware/react-renderer.js")>]
let reactRendererMiddleware : {| app: ExpressApp; appLayout: obj -> ReactElement |} -> unit = jsNative

[<Emit("app.use($0)")>]
let useMiddleware middleware: unit = jsNative
