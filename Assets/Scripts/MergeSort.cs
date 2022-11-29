using System.Collections.Generic;
using UnityEngine.Profiling;

public class MergeSort : Sorters
{
    protected new void Start()
    {
        base.Start();
        Sampler = CustomSampler.Create("MergeSort Sampler");
    }
    
    public override void BeginSorting()
    {
        Sampler.Begin();
        ListToBeSorted = SplitList(ListToBeSorted, 0, ListToBeSorted.Count - 1);
        Sampler.End();
    }

    private List<Ball> SplitList(List<Ball> list, int left, int right)
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;
            
            SplitList(list, left, middle);
            SplitList(list, middle + 1, right);
            
            MergeList(list, left, middle, right);
        }
        return list;
    }

    private void MergeList(List<Ball> list, int left, int middle, int right)
    {
        var leftArrayLength = middle - left + 1;
        var rightArrayLength = right - middle;
            
        var leftTempArray = new Ball[leftArrayLength];
        var rightTempArray = new Ball[rightArrayLength];
            
        int i, j;
            
        for (i = 0; i < leftArrayLength; ++i)
            leftTempArray[i] = list[left + i];
        for (j = 0; j < rightArrayLength; ++j)
            rightTempArray[j] = list[middle + 1 + j];
            
        i = 0;
        j = 0;
        int k = left;
        while (i < leftArrayLength && j < rightArrayLength)
        {
            if (leftTempArray[i].distanceToBaller <= rightTempArray[j].distanceToBaller)
            {
                list[k++] = leftTempArray[i++];
            }
            else
            {
                list[k++] = rightTempArray[j++];
            }
        }
        while (i < leftArrayLength)
        {
            list[k++] = leftTempArray[i++];
        }
        while (j < rightArrayLength)
        {
            list[k++] = rightTempArray[j++];
        }
    }
}

