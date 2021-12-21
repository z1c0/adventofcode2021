# Advent of Code 2021

<https://adventofcode.com/2021>

## Day 18: Snailfish

This took me a bit longer than it should have, but I quite enjoyed this puzzle.
I made 3 errors that took me quite a while:

* I had a silly mistake in my **tree-traversal** code to figure out **right** or **left**
neighbors of a number.
* I should have paid more attention to the **specification** here. In
my `Reduce` step, I made the mistake of applying the **first** reduction (`Explode` or `Split`)
I could find, however the specification clearly says to do *all* `Explode` first 🤦‍♂️.
* The **second part** was about finding the maximum of to snailfish number sums.
It turned out, the `+` operator I had implemented for my `SnailfishNumber` class
had a nasty **side-effect** that mutated its operands 😱. I was quite irritated
who this (apparently) easy second part would not yield the correct solution until
I figured this out.

## Day 19: Beacon Scanner

This was the first puzzle of that year that I could not solve on the day
it came out. I didn't even manage to solve the first part. I found a
solution that worked for the sample data, but it failed on the
actual input because of algorithmic complexity - basically, it did not finish.

