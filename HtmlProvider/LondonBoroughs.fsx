#r "nuget: FSharp.Data"
#r "nuget: Plotly.NET"

open FSharp.Data
open Plotly.NET

type LondonBoroughs = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_London_boroughs">

let boroughs = (LondonBoroughs.GetSample().Tables.``get_List of boroughs and local authoritiesedit``) ()

match (LondonBoroughs.GetSample().Tables.``get_City of Londonedit`` ()).Headers with
| Some headers -> headers |> Array.iter (fun h -> printfn "%s" h)
| None -> printfn "No headers"

(LondonBoroughs.GetSample().Tables.``get_City of Londonedit`` ()).Rows
|> Array.iter (fun row -> printfn "%A" row)

let density = 
    boroughs.Rows
    |> Array.map (fun row -> 
      row.Borough,
      row.``Population (2019 est)`` / row.``Area (sq mi)``
    )

density
|> Array.sortBy snd
|> Chart.Column
|> Chart.show