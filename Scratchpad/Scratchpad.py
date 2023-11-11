from typing import List, Callable, Optional, Any

class OrderLine:
    def __init__(self, id: int, price: float):
        self.id = id
        self.price = price

class Order:
    def __init__(self, id: int, order_lines: List[OrderLine]):
        self.id = id
        self.order_lines = order_lines

NthElement = Callable[[int], Callable[[List[Any]], Optional[Any]]]

FindOrderLine = Callable[[int], Callable[[List[OrderLine]], Optional[OrderLine]]]

ReplaceOrderLine = Callable[[int, OrderLine], Callable[[List[OrderLine]], List[OrderLine]]]

ChangeOrderLinePrice = Callable[[Order], Callable[[int], Callable[[float], Order]]]

nth_element: NthElement = lambda n: lambda list: (
    list[n] if n < len(list) else None)

find_order_line: FindOrderLine = lambda order_line_id: lambda order_lines: (
    next((ol for ol in order_lines if ol.id == order_line_id), None ))

replace_order_line: ReplaceOrderLine = lambda order_line_id, new_order_line: lambda order_lines: (
    [new_order_line if ol.id == order_line_id else ol for ol in order_lines ])

change_order_line_price: ChangeOrderLinePrice = lambda order: lambda order_line_id: lambda new_price: (
    Order(
        order.id,
        replace_order_line(order_line_id, OrderLine(order_line_id, new_price))(
            order.order_lines
    )))

order = Order(
    1,
    [OrderLine(1, 10.0), OrderLine(2, 20.0)])

updated_order = change_order_line_price(order)(1)(15.0)

order_line = nth_element(0)(updated_order.order_lines)
print(
    order_line.price
    if order_line
    else "The list does not have an element at that index"
)
