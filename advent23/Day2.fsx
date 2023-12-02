#load "Fetch.fsx"

open Fetch
open System.Text.RegularExpressions

let text = fetch "https://adventofcode.com/2023/day/2/input"

let exampleA = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"

let maxColors game = 
  game |> List.fold (fun (r, g, b) (color, count) ->
  match color with
  | "red" -> (max r count, g, b)
  | "green" -> (r, max g count, b)
  | "blue" -> (r, g, max b count)
  | _ -> (r, g, b)) (0, 0, 0)

let games = 
    text.Split('\n')
      |> Array.map (fun s -> Regex(@"(?<count>\d+) (?<color>red|green|blue)").Matches(s)) 
      |> Array.map (fun m -> m |> Seq.map (fun g -> (g.Groups.["color"].Value, int g.Groups.["count"].Value)) |> Seq.toList)
      |> Array.filter (fun game -> game.Length > 0)

let a = games
      |> Array.fold (fun (gameNumber, sumOfIdsOfGames) game ->
          match maxColors game with
          | (maxRed, maxGreen, maxBlue) when maxRed <= 12 && maxGreen <= 13 && maxBlue <= 14 -> 
              (gameNumber + 1, sumOfIdsOfGames + gameNumber)
          | _ -> 
              (gameNumber + 1, sumOfIdsOfGames)
        ) (1, 0)
      |> snd

printfn "%A" a

let b = games
      |> Array.map (fun game -> let (maxRed, maxGreen, maxBlue) = maxColors game in maxRed * maxGreen * maxBlue)
      |> Array.sum

printfn "%A" b
