using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SortingController : MonoBehaviour
{
    private ExperimentEnvironmentSetup _environment;
    private CSVWriter _csv;

    private bool _stopWriting;
    
    private Sorters _sorters; // Currently only set at start! --- THIS WILL GIVE ME BIG PAIN LATER! ---
    private PerformanceRecorder _performanceRecorder;
    private List<int> _ballCount;
    private List<float> _insertSortAverageData;
    private List<float> _icbcsSortAverageData;
    private List<float> _mergetSortAverageData;

    [SerializeField] private int amountOfBallsToAddPerIteration = 100;
    [SerializeField] private int sampleMax = 400;
    [SerializeField] private float sampleFailCap = 0.004f;
    private int _sampleCount;

    [Header("Select which type of sorting method gets used.")]
    [SerializeField] private bool ManualSelect;
    [SerializeField] private bool InsertSort;
    [SerializeField] private bool IcbcsSort;
    [SerializeField] private bool MergeSort;

    private bool _breakOperation;

    private void OnValidate()
    {
        if (sampleFailCap <= 0)
        {
            Debug.LogError("SampleFailCap set to 0 or less! SampleFailCap has to be greater than 0.");
        }
    }

    void Start()
    {
        if (TryGetComponent(out ExperimentEnvironmentSetup environment))
            _environment = environment;
        else
            Debug.LogError("Environment is not set.");

        if (TryGetComponent(out CSVWriter csvWriter))
        {
            _csv = csvWriter;
        }
        
        _ballCount = new List<int>();
            
        if (ManualSelect)
        {
            if (InsertSort)
            {
                if (TryGetComponent(out InsertSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
                _insertSortAverageData = new List<float>();
            }
            else if (IcbcsSort)
            {
                if (TryGetComponent(out IcbcsSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
                _icbcsSortAverageData = new List<float>();
            }
            else if (MergeSort)
            {
                if (TryGetComponent(out MergeSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
                _mergetSortAverageData = new List<float>();
            }
            else
            {
                Debug.LogError("Manual selection mode enabled but no sorting method chosen!");
            }
        }
        else
        {
            if (TryGetComponent(out InsertSort sort)) 
                        _sorters = sort;
            else
                Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
            InsertSort = true;
            _insertSortAverageData = new List<float>();
            _icbcsSortAverageData = new List<float>();
            _mergetSortAverageData = new List<float>();
        }
        
        _performanceRecorder = GetComponent<PerformanceRecorder>();
    }

    void Update()
    {
        if (_sampleCount < sampleMax && !_breakOperation)
        {
            StartSorting();
            SetBallColours();
        }
        else if (!_breakOperation)
        {
            ResetForNewSamples();
        }
        else
        {
            Profiler.enabled = false; // For debugging
            
            // Set up the next test with a new algorithm.
            _breakOperation = false;
            _sampleCount = 0;
            _environment.FullEnvironmentReset();
            
            if (ManualSelect) // If we have enabled manual mode we don't want to change the mode of sorting. Instead we want to end it.
            {
                _breakOperation = true;
                Debug.Log("Test ended.");
                return;
            }
            
            if (_sorters is InsertSort)
            {
                InsertSort = false;
                IcbcsSort = true;
                MergeSort = false;
                if (TryGetComponent(out IcbcsSort sort))
                    _sorters = sort;
            }
            else if (_sorters is IcbcsSort)
            {
                InsertSort = false;
                IcbcsSort = false;
                MergeSort = true;
                if (TryGetComponent(out MergeSort sort))
                    _sorters = sort;
            }
            else
            {
                _breakOperation = true;
                Debug.Log("Test ended.");
                if (!_stopWriting)
                {
                    WriteResultsToCSV();
                }
            }
        }
    }

    private void StartSorting()
    {
        _sampleCount++;

        if (_sorters)
        {
            _sorters.UpdateList();
            _sorters.BeginSorting();
        }
        else
        {
            Debug.LogWarning("Sorters is not set to an instance of an object and is thus unable to update!");
            return;
        }
            

        _performanceRecorder.Record(_sorters);

        if (_performanceRecorder.averageTimeForCurrentSample > sampleFailCap) // Compare to the average time for the current sample. IF it takes too long to sort it breaks the operation.
        {
            _breakOperation = true;
        }
    }

    private void ResetForNewSamples()
    {
        if (!_ballCount.Contains(_environment.ballSpawnAmount))
        {
            _ballCount.Add(_environment.ballSpawnAmount);
        }
        
        if (InsertSort)
        {
            _insertSortAverageData.Add(_performanceRecorder.averageTimeForCurrentSample);
        }
        else if (IcbcsSort)
        {
            _icbcsSortAverageData.Add(_performanceRecorder.averageTimeForCurrentSample);
        }
        else if (MergeSort)
        {
            _mergetSortAverageData.Add(_performanceRecorder.averageTimeForCurrentSample);
        }

        _environment.ballSpawnAmount += amountOfBallsToAddPerIteration;
        _environment.ResetEnvironment(); // TODO: Implement a reset function.
        _performanceRecorder.ClearAverageList(); // Clear the average list for new samples.
        _sampleCount = 0;
    }

    private void SetBallColours()
    {
        if (!_sorters)
        {
            Debug.LogWarning("Unable to access sorter! Can not set colours of the balls!");
            return;
        }

        List<Ball> balls = _sorters.GetResults();
        int halfBall = Mathf.FloorToInt(balls.Count / 2);
        
        for (int i = 0; i < balls.Count; i++)
        {
            if (i <= halfBall) balls[i].SetBallToRed();
            else balls[i].SetBallToWhite();
        }
    }

    private void WriteResultsToCSV()
    {
        _csv.WriteToCSV(_ballCount, _insertSortAverageData, _icbcsSortAverageData, _mergetSortAverageData);
        _stopWriting = true;
    }
}
