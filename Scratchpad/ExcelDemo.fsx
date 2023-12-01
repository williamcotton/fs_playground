#r "nuget: FsExcel"

#load "Unix.fsx"

open System.IO
open FsExcel
open System.Text
open Unix

let cells = 
  [
      for i in 1..10 do
          Cell [ Integer i ]
  ]

let ms = new MemoryStream()
cells |> Render.AsStream ms
ms.Seek(0L, SeekOrigin.Begin) |> ignore

let stdout = System.Console.OpenStandardOutput()
ms.CopyTo(stdout)