#!/usr/bin/env dotnet fsi

#load "DatabaseUtils.fsx"

open DatabaseUtils

connectToDatabase "Host=localhost;Database=express-test"
|> executeQuery "SELECT * FROM employees"
|> readResults
|> convertToJson
|> printfn "%s"
