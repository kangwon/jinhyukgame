using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomRandom<T>
{
    public static T WeightedChoice(List<T> list, List<double> weight, Random random)
    {
        if (list.Count != weight.Count)
            throw new ArgumentException("list and weight must have the same length.");
        
        var prob = random.NextDouble();
        var sum = weight.Sum();
        var cumsum = 0.0;
        for (int i = 0; i < list.Count; i++)
        {
            cumsum += weight[i];
            if (prob < (cumsum / sum))
                return list[i];
        }
        return list[list.Count - 1];
    }

    public static T Choice(List<T> list, Random random)
    {
        return CustomRandom<T>.WeightedChoice
        (
            list,
            Enumerable.Repeat(1.0, list.Count).ToList(),
            random
        );
    }
}
