#!/usr/bin/env dotnet fsi

#load "Unix.fsx"
open Unix

Ok "pattern
    runner
    batter"
    |> grep "tt"
    |> awk """'{print $1 " " $1 " " $1}'"""
    |> echo

Ok "one,two,three,date
    1,2,3,2021-01
    4,5,6,2021-02
    7,8,2,2021-03"
    |> zsh "
        plt '[one, two, three], date { bar 10px [solid red, solid green, solid blue] }' |
        imgcat"
    |> echo