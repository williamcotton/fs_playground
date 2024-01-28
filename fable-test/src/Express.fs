module Express

open Feliz
open Fable.Core
open Fable.Import.Express
open Fable.Core.JsInterop

type ExpressReq =
  abstract member params : obj
  abstract member body : obj
  abstract member query : obj
  abstract member Link : obj -> ReactElement
  abstract member Form : obj -> ReactElement

type ExpressRes =
  abstract member send : obj -> unit
  abstract member renderComponent : ReactElement -> unit

type ExpressApp =
  abstract member get: string * (ExpressReq -> ExpressRes -> unit -> unit) -> unit
  abstract member post: string * (ExpressReq -> ExpressRes -> unit -> unit) -> unit
  abstract member listen: int * (unit -> unit) -> unit

[<Emit("console.log($0)")>]
let consoleLog text: unit = jsNative