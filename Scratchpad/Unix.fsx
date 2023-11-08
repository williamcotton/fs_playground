open System
open System.Diagnostics
open System.Threading.Tasks

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
    executeCommand "/usr/bin/env" [ "-S"; "zsh"; "-c"; command ]

let executeUnixCommand command input =
    let fullCommand = sprintf "echo \"%s\" | %s" input command
    executeShellCommand fullCommand |> Async.RunSynchronously

let p command input = 
  match input with
  | Some i -> 
      let result = executeUnixCommand command i
      if result.ExitCode = 0 then Some result.StandardOutput else None
  | None -> None

let echo =
  Option.iter (printfn "%s")

let runPipeline =
  Some "
    pattern
    runner
    fatter
  "
  |> p "grep tt"
  |> p "awk '{print $1 $1 $1}'"
  |> echo

runPipeline