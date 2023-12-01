let processItem (accList: int list) currentItem =
    let sumSoFar = match accList with
                   | [] -> 0
                   | head :: _ -> head
    let newSum = sumSoFar + currentItem
    newSum :: accList

let initialList = []
let numbers = [1; 2; 3]

let cumulativeSums = List.fold processItem initialList numbers |> List.rev
printfn "%A" cumulativeSums