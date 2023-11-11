#!/usr/bin/env dotnet fsi

type OrderLine = { 
    Id: int; 
    Price: decimal 
}

type Order = {
    Id: int; 
    OrderLines: OrderLine list 
}

let nthElement n list = 
    try Some(List.item n list) with _ -> None

let findOrderLine orderLineId (orderLines: list<OrderLine>) =
    orderLines |> List.find (fun ol -> ol.Id = orderLineId)

let replaceOrderLine orderLineId (newOrderLine: OrderLine) (orderLines: list<OrderLine>) =
    orderLines |> List.map (fun ol -> if ol.Id = orderLineId then newOrderLine else ol)

let changeOrderLinePrice order orderLineId newPrice =
    let orderLine = order.OrderLines |> findOrderLine orderLineId
    let newOrderLine = { orderLine with Price = newPrice }
    let newOrderLines = order.OrderLines |> replaceOrderLine orderLineId newOrderLine
    let newOrder = { order with OrderLines = newOrderLines }
    newOrder


let order =
    { Id = 1
      OrderLines = [ { Id = 1; Price = 10.0M }; { Id = 2; Price = 20.0M } ] }

let updatedOrder = changeOrderLinePrice order 1 15.0M

match nthElement 0 updatedOrder.OrderLines with
| Some orderLine -> printfn "%A" orderLine.Price
| None -> printfn "The list does not have an element at that index"
