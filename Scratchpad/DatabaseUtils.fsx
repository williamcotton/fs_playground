module DatabaseUtils

#r "nuget: Npgsql"
#r "nuget: Newtonsoft.Json"

open Npgsql
open System.Collections.Generic
open Newtonsoft.Json

let connectToDatabase connectionString =
    let connection = new NpgsqlConnection(connectionString)
    connection.Open()
    connection

let executeQuery query connection =
    let command = new NpgsqlCommand(query, connection)
    command.ExecuteReader() :?> NpgsqlDataReader

let readResults (reader: NpgsqlDataReader) =
    let results = new List<Dictionary<string, obj>>()
    while reader.Read() do
        let row = new Dictionary<string, obj>()
        for i in 0 .. reader.FieldCount - 1 do
            row.[reader.GetName(i)] <- reader.GetValue(i)
        results.Add(row)
    results

let convertToJson results =
    JsonConvert.SerializeObject(results, Formatting.Indented)