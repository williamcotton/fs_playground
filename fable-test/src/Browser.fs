module Browser

open Browser.Dom
open Feliz
open Fable.Core
open App
open Express

[<Import("default", "browser-express")>]
let express : unit -> ExpressApp = jsNative

[<Import("default", "./browser/middleware/express-link.js")>]
let expressLinkMiddleware : unit -> unit = jsNative

[<Import("default", "./browser/middleware/react-renderer.js")>]
let reactRendererMiddleware : {| app: ExpressApp; appLayout: {| content: ReactElement; req: ExpressReq |} -> ReactElement |} -> unit = jsNative

[<Emit("app.use($0)")>]
let useMiddleware middleware: unit = jsNative

let app = express()
useMiddleware(expressLinkMiddleware())
useMiddleware(reactRendererMiddleware({| app = app; appLayout = AppLayout |}))

universalApp app

app.listen(3000, fun _ ->
    printfn "Listening on port 3000"
)
