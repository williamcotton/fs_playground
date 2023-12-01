#load "Fetch.fsx"

open Fetch
open System.Text.RegularExpressions

let text = fetch "https://adventofcode.com/2023/day/1/input"

let exampleA = "1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet"

let a = text.Split('\n') 
        |> Array.map (fun s -> Regex(@"\d").Matches(s)) 
        |> Array.map (fun m -> m |> Seq.map (fun g -> g.Value) |> Seq.toList) 
        |> Array.map (function
            | first :: rest when List.length rest > 0 -> first + List.last rest
            | first :: _ -> first + first
            | _ -> "0")
        |> Array.map (fun s -> int s)
        |> Array.sum

printfn "%A" a

let exampleB = "two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
fivecgtwotwo3oneighth"

let numberWordToDigitString stringNum =
    match stringNum with
    | "one" -> "1"
    | "two" -> "2"
    | "three" -> "3"
    | "four" -> "4"
    | "five" -> "5"
    | "six" -> "6"
    | "seven" -> "7"
    | "eight" -> "8"
    | "nine" -> "9"
    | _ -> stringNum

let lineTransform first last = numberWordToDigitString first + numberWordToDigitString last

let b = text.Split('\n')
        |> Array.map (fun s -> Regex(@"(?=(one|two|three|four|five|six|seven|eight|nine|\d))").Matches(s))
        |> Array.map (fun m -> m |> Seq.map (fun g -> g.Groups.[1].Value) |> Seq.toList)
        |> Array.map (function
            | first :: rest when List.length rest > 0 -> lineTransform first (List.last rest)
            | first :: _ -> lineTransform first first
            | _ -> "0")
        |> Array.map (fun s -> int s)
        |> Array.sum

printfn "%A" b