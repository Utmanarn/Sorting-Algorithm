using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class PerformanceRecorder : MonoBehaviour
{
    private ExperimentEnvironmentSetup _environment;
    private CSVWriter _csv;
    private Recorder _recorder;
    private float _elapsedMilliSec;
    private List<float> _elapsedMilisecondsList;
    public float averageTimeForCurrentSample;

    private string _sorterType = "";

    private void Start()
    {
        _environment = GetComponent<ExperimentEnvironmentSetup>();
        _csv = GetComponent<CSVWriter>();
        _elapsedMilisecondsList = new List<float>();
    }

    public void Record(Sorters sorters)
    {
        if (sorters is InsertSort) // Might move this to a different class or method later.
        {
            _recorder = Recorder.Get("InsertSort Sampler");
            _sorterType = "InsertSort";
        }
        else if (sorters is IcbcsSort)
        {
            _recorder = Recorder.Get("Icbcs Sampler");
            _sorterType = "IcbcsSort";
        }
        else if (sorters is MergeSort)
        {
            _recorder = Recorder.Get("MergeSort Sampler");
            _sorterType = "MergeSort";
        }

        if (_recorder.isValid)
        {
            _elapsedMilliSec = _recorder.elapsedNanoseconds * 0.000001f; // Solved: "Yes, it did take the last frame, so it could not be in fixed update." This doesn't work for some reason? Maybe because it takes last frame time? "https://docs.unity3d.com/ScriptReference/Profiling.Recorder-elapsedNanoseconds.html"
            _elapsedMilisecondsList.Add(_elapsedMilliSec);
            AverageMilliSec();
            PrintElapsedTimeForSorter();

            _csv.WriteToCSV(_sorterType, _environment.ballSpawnAmount , _elapsedMilliSec);
        }
        else
            Debug.LogWarning("Recorder is invalid.");
    }
    
    private void PrintElapsedTimeForSorter()
    {
        Debug.Log("Time elapsed: " + _elapsedMilliSec);
    }

    private void AverageMilliSec() // Get the Average performance of the algorithm in milliseconds
    {
        averageTimeForCurrentSample = _elapsedMilisecondsList.Average();
    }

    public void ClearAverageList() // TODO: Clear after the sample is finished
    {
        _elapsedMilisecondsList.Clear();
    }
}
