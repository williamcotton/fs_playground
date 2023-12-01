#load "Fetch.fsx"

open Fetch
open System.Text.RegularExpressions
  
let text = fetch "https://adventofcode.com/2023/day/1/input"

let a = text.Split('\n') 
        |> Array.map (fun s -> Regex(@"\d").Matches(s)) 
        |> Array.map (fun m -> m |> Seq.map (fun g -> g.Value) |> Seq.toList) 
        |> Array.map (function
            | first :: rest when List.length rest > 0 -> first ^ List.last rest
            | first :: _ -> first ^ first
            | _ -> "0")
        |> Array.map (fun s -> int s)
        |> Array.fold (+) 0

printfn "%A" a