using System;
using System.Collections.Generic;
namespace GetTogethers
{
    static public class ForEach
    {
        static public int ForEachWCount<T>(this IEnumerable<T> source, Action<T> action) where T : class
        {
            int result = 0;
            foreach(var t in source) {
                result++;
                action(t);
            }
            return result;
        }
    }
}
