#load "Fetch.fsx"

open Fetch
open System.Text.RegularExpressions

let day3Input = fetch "https://adventofcode.com/2023/day/3/input"

let exampleA = "467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598.."

let to2DArray (input: string) =
    input.Split('\n')
    |> Array.map (fun s -> s.ToCharArray())
    |> Array.map (fun a -> Array.map (fun c -> c.ToString()) a)

type SchematicMatch = { Value: string; Start: (int * int); End: (int * int); Range: (int * int) list; }

let getMatchCoordinates (input: string) (pattern: string) =
    let regex = Regex(pattern)
    input.Split('\n')
    |> Array.mapi (fun i line ->
        regex.Matches(line)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> { Value = m.Value; Start = (i, m.Index); End = (i, m.Index + m.Length - 1); Range = [for j in [m.Index .. m.Index + m.Length - 1] -> (i, j)] })
        |> Seq.toList)
    |> Array.toList
    |> List.concat    

to2DArray day3Input
    |> Array.mapi (fun i a -> a |> Array.mapi (fun j c -> printf "[%d,%d]%s " i j c) |> ignore; printfn "")

let symbolCoordinates = getMatchCoordinates exampleA @"[^.|\d]"
let numberCoordinates = getMatchCoordinates exampleA @"\d+"

printfn "%A" symbolCoordinates

let adjacentToSymbols symbolCoordinates =
    Array.ofList symbolCoordinates
    |> Array.map(fun s ->
        let (i, j) = s.Start
        let adjacent =
            [for i'' in [i - 1 .. i + 1] do
                for j'' in [j - 1 .. j + 1] do
                    if i'' >= 0 && j'' >= 0 && (i'' <> i || j'' <> j) then yield (i'', j'')]
        adjacent)
    |> Array.toList
    |> List.concat
    |> Set.ofSeq

let setToList = Set.toList (adjacentToSymbols symbolCoordinates)
for element in setToList do
    printfn "%A" element

// let a = (numberCoordinates
//     |> List.fold (fun acc index ->
//             printfn "%A" index
//             let listExists = List.exists (fun x -> Set.contains x (adjacentToSymbols symbolCoordinates)) index.Range
//             printfn "%A" listExists
//             if listExists then acc + int index.Value
//             else acc) 0)

// printfn "%A" a

let gearCoordinates = getMatchCoordinates exampleA @"\*"

let aroundGearCoordinates = adjacentToSymbols gearCoordinates

let b = aroundGearCoordinates
        |> Set.toList
        |> List.fold (fun acc index ->
            printfn "%A" index
            let hits = numberCoordinates
                       |> List.map (fun x -> List.contains index x.Range)
            printfn "%A" hits
            acc) 0