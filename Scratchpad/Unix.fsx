#r "nuget: SixLabors.ImageSharp"

open System
open System.Diagnostics
open System.IO
open System.Threading.Tasks
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats.Png

type CommandResult =
  { ExitCode: int
    StandardOutput: string
    StandardError: string }

let executeCommand executable args =
  async {
    let! ct = Async.CancellationToken

    let startInfo = ProcessStartInfo()
    startInfo.FileName <- executable
    startInfo.RedirectStandardOutput <- true
    startInfo.RedirectStandardError <- true
    startInfo.UseShellExecute <- false
    startInfo.CreateNoWindow <- true
    for a in args do
      startInfo.ArgumentList.Add(a)

    use p = new Process()
    p.StartInfo <- startInfo
    p.Start() |> ignore

    let outTask =
      Task.WhenAll([|
        p.StandardOutput.ReadToEndAsync(ct);
        p.StandardError.ReadToEndAsync(ct) |])

    do! p.WaitForExitAsync(ct) |> Async.AwaitTask
    let! out = outTask |> Async.AwaitTask

    return
      { ExitCode = p.ExitCode
        StandardOutput = out.[0]
        StandardError = out.[1] }
  }

let executeShellCommand command =
    executeCommand "/usr/bin/env" [ "-S"; "zsh"; "-c"; "source ~/.zshrc; " + command ]

let executeUnixCommand command input =
    let fullCommand = sprintf "echo \"%s\" | %s" input command
    executeShellCommand fullCommand |> Async.RunSynchronously

let p command input = 
  match input with
  | Ok i -> 
      let result = executeUnixCommand command i
      if result.ExitCode = 0 then Ok result.StandardOutput else Error result.StandardError
  | Error e -> Error e

let echo = function
  | Ok i -> printfn "%s" i
  | Error e -> printfn "%s" e

let runPipelineAwk =
  Ok "
    pattern
    runner
    fatter
  "
  |> p "grep tt"
  |> p "nawk '{print $1 $1 $1}'"
  |> echo

runPipelineAwk


let runPipelinePlt =
  Ok "one,two,date\n1,2,2021-01\n3,4,2021-02"
  |> p "plt 'one, date { plot 1px dashed red }' | imgcat"
  |> echo

runPipelinePlt