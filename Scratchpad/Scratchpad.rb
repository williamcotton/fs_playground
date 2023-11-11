require 'dry-struct'
require 'dry-types'

module Types
  include Dry.Types()
end

class OrderLine < Dry::Struct
  attribute :id, Types::Integer
  attribute :price, Types::Float
end

class Order < Dry::Struct
  attribute :id, Types::Integer
  attribute :order_lines, Types::Array.of(OrderLine)
end

nth_element = -> (n, list) {
  list[n] rescue nil }

find_order_line = -> (order_line_id) { -> (order_lines) { 
  order_lines.find { |ol| ol.id == order_line_id } } }

replace_order_line = -> (order_line_id) { -> (new_order_line) { -> (order_lines) {
  order_lines.map { |ol| ol.id == order_line_id ? new_order_line : ol } } } }

change_order_line_price = -> (order) { -> (order_line_id) { -> (new_price) {
      order_line = find_order_line.call(order_line_id).call(order.order_lines)
      new_order_line = OrderLine.new(id: order_line&.id, price: new_price)
      new_order_lines = replace_order_line.call(order_line_id).call(new_order_line).call(order.order_lines)
      Order.new(id: order.id, order_lines: new_order_lines) } } }

order = Order.new(
  id: 1, 
  order_lines: [OrderLine.new(id: 1, price: 10.0), OrderLine.new(id: 2, price: 20.0)])

updated_order = change_order_line_price.call(order).call(1).call(15.0)

order_line = nth_element.call(0, updated_order.order_lines)
puts order_line ? 
  order_line.price : 
  "The list does not have an element at that index"
