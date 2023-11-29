using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    private string _filePath;
    
    // Start is called before the first frame update
    void Start()
    {
        _filePath = Application.dataPath + "/output.csv";
        CreateCSV();
    }

    private void CreateCSV()
    {
        using (StreamWriter streamWriter = new StreamWriter(_filePath, false))
        {
            streamWriter.WriteLine("BallCount; InsertSort; ICBCS; MergeSort");
            streamWriter.Close();
        }
    }

    public void WriteToCSV(List<int> ballCount, List<float> insertSort, List<float> icbcsSort, List<float> mergeSort)
    {
        using (StreamWriter streamWriter = new StreamWriter(_filePath, true))
        {
            for (int i = 0; i < ballCount.Count; i++)
            {
                streamWriter.Write(ballCount[i] + "; ");
                if (insertSort.Count > i)
                {
                    streamWriter.Write(insertSort[i] + "; ");
                }
                else
                {
                    streamWriter.Write("0" + "; ");
                }
                if (icbcsSort.Count > i)
                {
                    streamWriter.Write(icbcsSort[i] + "; ");
                }
                else
                {
                    streamWriter.Write("0" + "; ");
                }
                if (mergeSort.Count > i)
                {
                    streamWriter.WriteLine(mergeSort[i]);
                }
                else
                {
                    streamWriter.WriteLine("0");
                }            
            }
            
            streamWriter.Close();
        }
    }
}
