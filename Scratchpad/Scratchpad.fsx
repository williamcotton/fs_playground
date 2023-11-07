#!/usr/bin/env dotnet fsi

#load "DatabaseUtils.fsx"

open DatabaseUtils

connectToDatabase "Host=localhost;Database=post_v_course"
|> executeQuery "SELECT title FROM document_titles LIMIT 10"
|> readResults
|> convertToJson
|> printfn "%s"
