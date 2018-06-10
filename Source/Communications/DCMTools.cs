using System;
using Verse;
namespace GetTogethers
{
    static public class DCMTools
    {
        public static DynamicCommManager DynamicCommManagerFor(Map map)
        {
            return map.GetComponent<DynamicCommManager>();
        }
    }
}
