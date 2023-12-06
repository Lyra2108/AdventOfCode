//Time:        46     85     75     82
//Distance:   208   1412   1257   1410
var input = new[]
{
    (Time: 46, Distance: 208),
    (Time: 85, Distance: 1412),
    (Time: 75, Distance: 1257),
    (Time: 82, Distance: 1410)
};

// Time:      7  15   30
// Distance:  9  40  200
var example = new[]
{
    (Time: 7, Distance: 9),
    (Time: 15, Distance: 40),
    (Time: 30, Distance: 200)
};


var product = input.Select(race => CalcOptions(race))
    .Select(options => options.Max - options.Min + 1)
    .Aggregate((x, y) => x * y);

Console.WriteLine($"The product is {product}");

var input2 = (Time: 46857582, Distance: 208141212571410);
var options = CalcOptions(input2);

Console.WriteLine($"There are {options.Max - options.Min + 1} options");

(double Min, double Max) CalcOptions((int Time, long Distance) race)
{
    var min = Math.Ceiling(race.Time / 2d - Math.Sqrt(Math.Pow(race.Time / 2d, 2) - race.Distance));
    var max = Math.Floor(race.Time / 2d + Math.Sqrt(Math.Pow(race.Time / 2d, 2) - race.Distance));
    return (min, max);
}
