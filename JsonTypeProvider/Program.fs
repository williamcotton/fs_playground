module Program

open JsonTypes

let json = """ { "Name": "John", "Age": "30" } """
let sample = JsonTypeProvider<_, SampleJson = json>.SampleType()

[<EntryPoint>]
let main argv =
    printfn "%A" sample.Name
    printfn "%A" sample.Age
    0 // return an integer exit code