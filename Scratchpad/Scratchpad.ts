type OrderLineId = string;
type OrderId = string;
type Price = number;
type OrderLine = {
  id: OrderLineId;
  price: Price;
};
type Order = {
  id: OrderId;
  orderLines: OrderLine[];
};

const changeOrderLinePrice =
  (order: Order) =>
  (orderLineId: OrderLineId) =>
  (newPrice: Price): Order => {
    let orderLine = order.orderLines.find((ol) => ol.id === orderLineId);
    let newOrderLine = { ...orderLine, price: newPrice } as OrderLine;
    let newOrderLines = order.orderLines.map((ol) =>
      ol.id === orderLineId ? newOrderLine : ol
    );
    let newOrder = { ...order, orderLines: newOrderLines };
    return newOrder;
  };

const order1: Order = {
  id: "1",
  orderLines: [
    { id: "1", price: 10 },
    { id: "2", price: 20 },
  ],
};
const newOrder1 = changeOrderLinePrice(order1)("1")(30);

const order2: Order = {
  id: "2",
  orderLines: [
    { id: "1", price: 10 },
    { id: "2", price: 20 },
  ],
};
const newOrder2 = changeOrderLinePrice(order2)("2")(10);

console.log(newOrder1.orderLines.find((ol) => ol.id === "1")?.price);