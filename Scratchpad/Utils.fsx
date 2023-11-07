#r "nuget: SixLabors.ImageSharp"

open System.Diagnostics
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats.Png

let csvData =
    "one,two,three,date
  1,2,3,2021-01
  4,5,6,2021-02
  7,8,9,2021-03"

let plt args (csvData: string) =
    let proc = new Process()

    // Set the command
    proc.StartInfo.FileName <- "/opt/homebrew/Caskroom/miniforge/base/bin/python"
    proc.StartInfo.Arguments <- sprintf "/Users/williamc/dotfiles/bin/plt \"%s\"" args
    proc.StartInfo.UseShellExecute <- false
    proc.StartInfo.RedirectStandardOutput <- true
    proc.StartInfo.RedirectStandardInput <- true
    proc.StartInfo.EnvironmentVariables["PYTHONPATH"] <- "/opt/homebrew/Caskroom/miniforge/base/bin/python"
    proc.Start() |> ignore
    proc.StandardInput.WriteLine(csvData)
    proc.StandardInput.Close()
    proc.WaitForExit()

    // Read the output
    let output = proc.StandardOutput.BaseStream
    let bytes = new MemoryStream()
    output.CopyTo(bytes)

    // Reset the position of the MemoryStream to the beginning
    bytes.Position <- 0L

    // Load the image from the bytes
    let image = Image.Load(bytes)
    image

let image = plt "one, date { plot 1px dashed red }" csvData