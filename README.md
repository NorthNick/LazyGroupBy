# LazyGroupBy
A lazier version of the .NET LINQ GroupBy function

[![Build Status](https://travis-ci.org/NorthNick/LazyGroupBy.svg?branch=master)](https://travis-ci.org/NorthNick/LazyGroupBy)

The .NET LINQ [GroupBy](https://msdn.microsoft.com/en-us/library/system.linq.enumerable.groupby(v=vs.100).aspx) function is
handy but, in order to to get exactly one group per key value, it eagerly consumes its source enumerable, converting it to a
Lookup. Sometimes you know that the source is already grouped by the key, in which case it's possible to output and consume the GroupBy
results lazily. This can save space by avoiding a potentially large intermediate Lookup object.

LazyGroupBy behaves just like GroupBy, except that it assumes that all values for a given key will be contiguous in the source,
so it can output results lazily. If one key's objects are scattered throughout the source, then the results will contain one
grouping for each contiguous group.

## Installation
The simplest way to use LazygroupBy is to take a copy of the LazyGroupBy project, compile it up to create the Shastra.LazyGroupBy
assembly, and add a reference in your project. Adding a "using Shastra.LazyGroupBy" statement in your code will then give access
to the LazyGroupBy method, which has exactly the same signatures as the usual groupBy one.

## Implementation
LazyGroupBy is written in C# 5, so can be compiled with any recent Visual Studio edition. It comes with a test project, using
NUnit 3. Code is not pushed to GitHub unless all tests pass successfully.

The project targets .NET 4.5.1. Any recent framework version should work; 4.5.1 is just the most recent version
supported by [Travis](https://travis-ci.org/NorthNick/LazyGroupBy) at the time of writing.

The LazyGroupBy project contains fewer than 100 lines of code, so documentation is sparse. The only point of difficulty is the
ListBackedEnumerable class, in which there are some subtleties to ensure that multiple iterators on the same IGrouping see the
same results.

