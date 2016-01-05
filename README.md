# LazyGroupBy
A lazier version of the .NET LINQ GroupBy function

The .NET LINQ [GroupBy](https://msdn.microsoft.com/en-us/library/system.linq.enumerable.groupby(v=vs.100).aspx) function is
handy but, in order to to get exactly one group per key value, it eagerly consumes its source enumerable, converting it to a
Lookup. Sometimes you know that the source is already grouped by the key, in which case it's possible to output the GroupBy
results lazily. This can save space by avoiding a potentially large intermediate Lookup object.

LazyGroupBy behaves just like GroupBy, except that it assumes that all values for a given key will be contiguous in the source,
so it can output results lazily. If one key's objects are scattered throughout the source, then the results will contain one
grouping for each contiguous group.

LazyGroupBy is a work in progress. It currently contains code that compiles but has not been tested.
