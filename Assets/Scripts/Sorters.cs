using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

// TODO: Create the list that has to be sorted. Must somehow take in the balls and save them so that they can be coloured depending on how close they are based on their place in the list.

public class Sorters : MonoBehaviour
{
    protected CustomSampler Sampler;
    protected GameObject TheBaller; // Might switch
    protected List<Ball> ListToBeSorted;

    protected void Start()
    {
        TheBaller = GameObject.Find("TheBaller");
        if (!TheBaller)
        {
            Debug.LogWarning("The Baller is not set.");
        }
        ListToBeSorted = new List<Ball>();
    }

    public void UpdateList()
    {
        ListToBeSorted.Clear();
        foreach (var ball in ExperimentEnvironmentSetup.Balls)
        {
            ListToBeSorted.Add(ball);
        }

        //ListToBeSorted = ExperimentEnvironmentSetup.Balls; // Copy ExperimentEnvironment list instead of sending it as a reference!
    }

    public virtual void BeginSorting() {}

    public List<Ball> GetResults()
    {
        return ListToBeSorted;
    }
}
