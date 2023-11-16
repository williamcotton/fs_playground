#!/bin/zsh

seq 1 10 |
  awk '{if ($1 % 3 == 0) print $1}' |
  awk '{print $1 * 2}' |
  paste -sd+ - | bc

seq 1 10 |
  awk '{if ($1 % 3 == 0) print $1 * 2}' |
  paste -s -d "+" - | bc

seq 1 10 |
  awk '{ if ($1 % 3 == 0) sum += ($1 * 2) }
    END { print sum }' # "36"