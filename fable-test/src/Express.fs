module Express

open Feliz

type ExpressReq =
  abstract member params : obj

type ExpressRes =
  abstract member send : obj -> unit
  abstract member renderComponent : ReactElement -> unit

type ExpressApp =
  abstract member get: string * (ExpressReq -> ExpressRes -> unit -> unit) -> unit
  abstract member listen: int * (unit -> unit) -> unit
