#include <stdio.h>
#include <stdlib.h>
#include <Block.h>

#define b Block_copy
#define r return

typedef struct OrderLine {
    int id;
    double price;
} OrderLine;

typedef struct Order {
    int id;
    OrderLine* orderLines;
    int lineCount;
} Order;

typedef void* (^ (^ (^NthElement)(int))(void*))(int);
typedef OrderLine* (^ (^ (^FindOrderLine)(int))(OrderLine*))(int);
typedef void (^ (^ (^ (^ReplaceOrderLine)(int))(OrderLine))(OrderLine*))(int);
typedef Order (^ (^ (^ChangeOrderLinePrice)(Order))(int))(double);

NthElement nthElement = ^(int n) { r b(^(void* list) { r b(^(int listCount) {
            r (n < listCount) ? (void*)&((char*)list)[n * sizeof(list[0])] : (void*)NULL; }); }); };

FindOrderLine findOrderLine = ^(int orderLineId) { r b(^(OrderLine* orderLines) { r b(^(int lineCount) {
            for (int i = 0; i < lineCount; ++i) {
                if (orderLines[i].id == orderLineId) {
                    r &orderLines[i];
                }
            }
            r (OrderLine*)NULL; }); }); };

ReplaceOrderLine replaceOrderLine = (^(int orderLineId) { r b(^(OrderLine newOrderLine) { r b(^(OrderLine* orderLines) { r b(^(int lineCount) {
                for (int i = 0; i < lineCount; ++i) {
                    if (orderLines[i].id == orderLineId) {
                        orderLines[i] = newOrderLine;
                        break;
                    } } }); }); }); });

ChangeOrderLinePrice changeOrderLinePrice = ^(Order order) { r b(^(int orderLineId) { r b(^(double newPrice) {
            OrderLine* orderLine = findOrderLine(orderLineId)(order.orderLines)(order.lineCount);
            if (orderLine != NULL) {
                OrderLine newOrderLine = { .id = orderLine->id, .price = newPrice };
                replaceOrderLine(orderLineId)(newOrderLine)(order.orderLines)(order.lineCount);
            }
            Order newOrder = { .id = order.id, .orderLines = order.orderLines, .lineCount = order.lineCount };
            r newOrder; }); }); };

int main() {
    Order order = {
      .id = 1, 
      .orderLines = (OrderLine[]){{.id = 1, .price = 10.0}, {.id = 2, .price = 20.0}}, .lineCount = 2};

    Order updatedOrder = changeOrderLinePrice(order)(1)(15.0);

    OrderLine* orderLine = nthElement(0)(updatedOrder.orderLines)(updatedOrder.lineCount);
    if (orderLine) {
        printf("OrderLine ID: %d, Price: %.2f\n", orderLine->id, orderLine->price);
    } else {
        printf("The list does not have an element at that index\n");
    }
}