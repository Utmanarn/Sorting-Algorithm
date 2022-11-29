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
        TextWriter writer = new StreamWriter(_filePath, false);
        writer.WriteLine("Algorithm.Ball Count.Execution Time");
        writer.Close();
    }

    public void WriteToCSV(string sortType, float ballCount, float sampleData)
    {
        TextWriter writer = new StreamWriter(_filePath, true);
        writer.WriteLine(sortType + "." + ballCount + "." + sampleData);
        writer.Close();
    }
}
