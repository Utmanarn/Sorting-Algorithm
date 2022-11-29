// I implemented this sorting algorithm "I can't believe it can sort" after finding out about it through this paper: https://arxiv.org/abs/2110.01111

using System.Collections.Generic;
using UnityEngine.Profiling;

public class IcbcsSort : Sorters
{
    protected new void Start()
    {
        base.Start();
        Sampler = CustomSampler.Create("Icbcs Sampler");
    }
    
    public override void BeginSorting()
    {
        Sampler.Begin();
        Sort(ListToBeSorted);
        Sampler.End();
    }

    private void Sort(List<Ball> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[i].distanceToBaller < list[j].distanceToBaller)
                {
                    Ball temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }
}
