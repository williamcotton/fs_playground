OrderLine = Struct.new(:id, :price)
Order = Struct.new(:id, :order_lines)

change_order_line_price = -> (order) {
  -> (order_line_id) {
    -> (new_price) {
      new_order_lines = order&.order_lines&.map do |ol|
        if ol.id == order_line_id
          OrderLine.new(ol.id, new_price)
        else
          ol
        end
      end
      Order.new(order&.id, new_order_lines)
    }
  }
}

order1 = Order.new('1', [OrderLine.new('1', 10), OrderLine.new('2', 20)])
new_order1 = change_order_line_price.call(order1).call('1').call(30)

order2 = Order.new('2', [OrderLine.new('1', 10), OrderLine.new('2', 20)])
new_order2 = change_order_line_price.call(order1).call('2').call(10)

puts new_order1.order_lines.find { |ol| ol.id == '2' }&.price
