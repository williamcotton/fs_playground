#r "nuget: Plotly.NET"

open Plotly.NET

Chart.Point(
    x = [0 .. 10],
    y = [0 .. 10]
)
|> Chart.withTitle "Hello World!"
|> Chart.show