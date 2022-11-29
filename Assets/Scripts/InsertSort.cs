using UnityEngine.Profiling;

public class InsertSort : Sorters
{
    protected new void Start()
    {
        base.Start();
        Sampler = CustomSampler.Create("InsertSort Sampler");
    }

    public override void BeginSorting()
    {
        Sampler.Begin();
        for (int i = 0; i < ListToBeSorted.Count; i++)
        {
            float temp = ListToBeSorted[i].distanceToBaller;
            int j = i - 1;
            while (j >= 0 && ListToBeSorted[j].distanceToBaller > temp)
            {
                Ball storageVar = ListToBeSorted[j + 1];
                ListToBeSorted[j + 1] = ListToBeSorted[j];
                ListToBeSorted[j] = storageVar;
                j--;
            }

            ListToBeSorted[j + 1].distanceToBaller = temp;
        }
        Sampler.End();
    }
}
