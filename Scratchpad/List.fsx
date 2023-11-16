[1; 2; 3; 4; 5; 6; 7; 8; 9; 10]
  |> List.filter (fun x -> x % 3 = 0)
  |> List.map (fun x -> x * 2)
  |> List.reduce (fun acc x -> acc + x)