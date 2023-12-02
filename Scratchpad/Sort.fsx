let list = [4; 2; 7; 1; 3]

let bubbleSort list =
    let rec sort lst = 
        match lst with
        | [] -> []
        | [x] -> [x]
        | x1 :: x2 :: xs -> 
            if x1 > x2 then x2 :: sort (x1 :: xs)
            else x1 :: sort (x2 :: xs)
    let rec repeatSort lst n =
        if n = 0 then lst
        else repeatSort (sort lst) (n - 1)
    repeatSort list (List.length list)

bubbleSort list

let selectionSort list =
  let rec findMinIndex list min idx minIdx i =
      match list with
      | [] -> minIdx
      | x :: xs ->
          if x < min then findMinIndex xs x (i + 1) i (i + 1)
          else findMinIndex xs min (i + 1) minIdx (i + 1)

  let swap list i j =
      if i = j then list
      else
          list
          |> Array.ofList
          |> fun arr ->
              let temp = arr.[i]
              arr.[i] <- arr.[j]
              arr.[j] <- temp
              arr
          |> Array.toList

  let rec sort sorted unsorted n =
      match unsorted with
      | [] -> sorted
      | _ ->
          let minIndex = findMinIndex unsorted (List.head unsorted) 0 0 0
          let newUnsorted = swap unsorted 0 minIndex |> List.tail
          sort (sorted @ [List.item minIndex unsorted]) newUnsorted (n + 1)

  sort [] list 0

selectionSort list

let rec selectionSortB lst =
    match lst with
    | [] -> [] // An empty list is already sorted
    | _ ->
        let minElement = List.min lst // Find the minimum element
        let restList = List.filter (fun x -> x <> minElement) lst // Remove the minimum element from the list
        minElement :: selectionSortB restList // Recursively sort the rest of the list and prepend the minimum element

selectionSortB list

let rec insertionSort unsortedList =
  let rec insert x sortedList =
      match sortedList with
      | [] -> [x]
      | head :: _ when x <= head -> x :: sortedList
      | head :: tail -> head :: insert x tail

  match unsortedList with
  | [] -> []
  | head :: tail -> insert head (insertionSort tail)

insertionSort list

let insertionSortB list =
  let rec insert x sortedList =
      match sortedList with
      | [] -> [x]
      | head :: _ when x <= head -> x :: sortedList
      | head :: tail -> head :: insert x tail

  let rec sort unsortedList sortedList =
      match unsortedList with
      | [] -> sortedList
      | head :: tail -> sort tail (insert head sortedList)

  sort list []

insertionSortB list