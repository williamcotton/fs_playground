module Browser

open Browser.Dom
open Feliz
open Fable.Core
open App

let root = ReactDOM.createRoot(document.getElementById "root")
root.render(Counter())
