module App

open Node.Api
open Feliz
open Fable.Core

[<ReactComponent>]
let Counter() =
    let (count, setCount) = React.useState(0)
    Html.div [
        Html.h1 count
        Html.button [
            prop.text "Increment123"
            prop.onClick (fun _ -> setCount(count + 1))
        ]
    ]