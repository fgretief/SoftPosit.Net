# SoftPosit.Net

.NET assembly for the C-based [SoftPosit](https://gitlab.com/cerlane/SoftPosit) library - a posit arithmetic emulator.


### Example

```
using System;
using System.Numerics;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Posit16 x = 1;
            Posit16 y = 1.5;
            Posit8 x8 = 1;

            x += (Posit16)1.5 * 5.1;

            Console.WriteLine("{0:F13}  sizeof: {1}", (double)x, sizeof(Posit16));

            var q = Quire16.Create();
            q += (4, 1.2);
            x = q.ToPosit();

            Console.WriteLine("{0:F13}  sizeof: {1}", (double)x, sizeof(Quire16));

            var q8 = Quire8.Create();
            q8 += (4, 1.2);
            x8 = q8.ToPosit();

            Console.WriteLine("{0:F13}  sizeof: {1}", (double)x8, sizeof(Quire8));

            // The example displays the following output:
            //      8.65  sizeof: 2
            //      4.8   sizeof: 16
            //      4.8   sizeof: 4
        }
    }
}
```
