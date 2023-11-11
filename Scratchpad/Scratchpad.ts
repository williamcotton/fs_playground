type OrderLine = {
  id: number,
  price: number
}

type Order = {
  id: number,
  orderLines: OrderLine[]
}

const nthElement = (n: number, list: any[]) =>
  list.length > n ? list[n] : null

const replaceOrderLine = (orderLineId: number) => (newOrderLine: OrderLine) => (orderLines: OrderLine[]) =>
  orderLines.map((ol) => (ol.id === orderLineId ? newOrderLine : ol))

const findOrderLine = (orderLineId: number) => (order: Order) => 
  order.orderLines.find((ol) => ol.id === orderLineId)

const changeOrderLinePrice = (order: Order) => (orderLineId: number) => (newPrice: number): Order => {
  let orderLine = findOrderLine(orderLineId)(order)
  let newOrderLine = { ...orderLine, price: newPrice } as OrderLine
  let newOrderLines = replaceOrderLine(orderLineId)(newOrderLine)(order.orderLines)
  let newOrder = { ...order, orderLines: newOrderLines }
  return newOrder
};

const order: Order = {
  id: 1,
  orderLines: [{ id: 1, price: 10.0 }, { id: 2, price: 20.0 }]}

const updatedOrder = changeOrderLinePrice(order)(1)(15.0)

const orderLine = nthElement(0, updatedOrder.orderLines)
console.log(
  orderLine
    ? orderLine.price
    : "The list does not have an element at that index"
)
