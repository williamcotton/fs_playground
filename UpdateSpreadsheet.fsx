#!/usr/bin/env dotnet fsi

#r "nuget: ClosedXML"

open ClosedXML.Excel

let tryLoadWorkbook (filePath: string) =
    try
        let workbook = new XLWorkbook(filePath)
        Ok workbook
    with
    | ex -> Error ex.Message

let tryGetWorksheet (workbook: XLWorkbook) (sheetName: string)  =
    try
        let worksheet = workbook.Worksheet(sheetName)
        if isNull worksheet then None else Some worksheet
    with
    | _ -> None

let getColumnLetter (worksheet: IXLWorksheet) columnName =
    let headers = worksheet.Row(1).CellsUsed() // Assuming the headers are in the first row
    headers 
    |> Seq.tryFind (fun cell -> cell.GetString() = columnName) 
    |> Option.map (fun cell -> cell.Address.ColumnLetter)

let processWorkbook workbook sheetName =
    match tryGetWorksheet workbook sheetName with
    | Some worksheet ->
        let fourColumnLetter = getColumnLetter worksheet "four"
        let fiveColumnLetter = getColumnLetter worksheet "five"
        let sixColumnLetter = getColumnLetter worksheet "six"

        match fourColumnLetter, fiveColumnLetter, sixColumnLetter with
        | Some four, Some five, Some six ->
            let rows = worksheet.RowsUsed()
            let matchedRow = 
                rows
                |> Seq.tryFind (fun row -> 
                    row.Cell(four).GetString() = "10" && 
                    row.Cell(five).GetString() = "11")

            match matchedRow with
            | Some row -> 
                row.Cell(six).SetValue("90") |> ignore
                printfn "Row updated successfully"
            | None -> printfn "No matching row found for four 10 and five 11"
        | _ -> printfn "Required columns not found"
    | None -> printfn "Worksheet not found"


let readAndProcessExcelFile filePath =
    match tryLoadWorkbook filePath with
    | Ok workbook ->
        processWorkbook workbook "Sheet2"
        workbook.Save()
        printfn "File updated successfully"
    | Error errMsg -> printfn "An error occurred: %s" errMsg

// File path and New Bates Number
let fileName = "demo.xlsx"

// Read, process, and update the file
readAndProcessExcelFile fileName
