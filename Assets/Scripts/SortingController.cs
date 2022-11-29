using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SortingController : MonoBehaviour
{
    private ExperimentEnvironmentSetup _environment;
    
    private Sorters _sorters; // Currently only set at start! --- THIS WILL GIVE ME BIG PAIN LATER! ---
    private PerformanceRecorder _performanceRecorder;

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

    void Start()
    {
        if (TryGetComponent(out ExperimentEnvironmentSetup environment))
            _environment = environment;
        else
            Debug.LogError("Environment is not set.");

        if (ManualSelect)
        {
            if (InsertSort)
            {
                if (TryGetComponent(out InsertSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
            }
            else if (IcbcsSort)
            {
                if (TryGetComponent(out IcbcsSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
            }
            else if (MergeSort)
            {
                if (TryGetComponent(out MergeSort sort)) 
                    _sorters = sort;
                else
                    Debug.LogWarning("_sorters not set to an instance of an object in SortingController!");
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
                return;
            }
            
            if (_sorters is InsertSort)
            {
                // Currently only for testing. Doesn't change anything.
                InsertSort = false;
                IcbcsSort = true;
                MergeSort = false;
                // ---------------------------------------------------
                if (TryGetComponent(out IcbcsSort sort))
                    _sorters = sort;
            }
            else if (_sorters is IcbcsSort)
            {
                // Currently only for testing. Doesn't change anything.
                InsertSort = false;
                IcbcsSort = false;
                MergeSort = true;
                // ---------------------------------------------------
                if (TryGetComponent(out MergeSort sort))
                    _sorters = sort;
            }
            else
            {
                _breakOperation = true; 
                Debug.Log("Test ended.");
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

    private void ShowResults()
    {
        /* TODO: Fix this to take in the right list.
        if (_sorters)
        {
            float[] array;
            
            array = _sorters.GetResults().ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                print("Array position " + i + " contains the number: " + array[i]);
            }
        }
        */
        
    }
}
