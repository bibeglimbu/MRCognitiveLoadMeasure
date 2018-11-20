using HoloToolkit.Unity;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System.IO;

public class CanvasModifier : MonoBehaviour, IInputClickHandler
{
    public Canvas canvas;

    /// <summary>
    /// the time when the color was changed and reaction is awaited
    /// </summary>
    DateTime startRecordingTime;

    /// <summary>
    /// Variable that holds the state when the data should be saved
    /// </summary>
    bool startedRecording = false;

    /// <summary>
    /// Random number generator for generating numbers between 
    /// </summary>
    System.Random rand = new System.Random();

    // Use this for initialization
    void Start () {
        //subscribe to the clicker clicked event
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }
    /// <summary>
    /// Event handler for the clicker clicked
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        ClickerClicked();
    }

    // Update is called once per frame
    void Update () {
        
    }

    /// <summary>
    /// registers the number of time the color was changed
    /// </summary>
    int numberOfChange = 0;
    /// <summary>
    /// Method that the event handler <see cref="OnInputClicked(InputClickedEventData)"/> calls 
    /// </summary>
    private void ClickerClicked()
    {
        //if the color has changed
        if (colorChanged == true)
        {
            numberOfChange += 1;
            //calculate the time taken to click based on the start recording time
            int totalTimeToReact = (DateTime.Now - startRecordingTime).Milliseconds;
            Debug.Log("Total time to react: " + totalTimeToReact);
            //turn  off the color
            canvas.transform.Find("RawImage 1").GetComponent<RawImage>().enabled = false;
            canvas.transform.Find("RawImage 2").GetComponent<RawImage>().enabled = false;
            //change the state
            colorChanged = false;

            //if the student is recording data
            if (startedRecording == true)
            {
                String s = "Time taken to react on change " + numberOfChange.ToString() + " : " + totalTimeToReact;
                //add the time data to the myrecording object frames holder
                myRecordingObject.frames.Add(s);
            }

        }
        else
        {
            if (startedRecording == true)
            {
                String s = "reacted while no circle was displayed";
                //add the time data to the myrecording object frames holder
                myRecordingObject.frames.Add(s);
            }
            Debug.Log("Click! but color not active");
        }
        
    }

    /// <summary>
    /// defines if the color was changed until the next iteration is called
    /// </summary>
    private bool colorChanged = false;
    /// <summary>
    /// Changes the canvas color to await the users interaction
    /// </summary>
    private void ChangeCanvasRandom()
    {
        int CircleNumber = rand.Next(1, 3);
        //make the circle visible and update the state
        canvas.transform.Find("RawImage " + CircleNumber).GetComponent<RawImage>().enabled = true;
        colorChanged = true;
        Debug.Log("Raw Image changed");
        //register the current time when the color is changed.
        startRecordingTime = DateTime.Now;
        int fortimerinterval = rand.Next(1, 6);
        //call the second iteration
        Invoke("ChangeCanvasRandom", fortimerinterval);
    }

    
    /// <summary>
    /// Method for generating the Json in one full sweep
    /// </summary>
    void WriteJson()
    {
        Debug.Log("number of stored frames" + myRecordingObject.frames.Count);
        string json = JsonUtility.ToJson(myRecordingObject);
        Debug.Log("json string" + json);
        string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        string filename = string.Format("ReactionTimer_{0}.json", timeStamp);
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        filepath = filepath.Replace("/", @"\");
        File.WriteAllText(filepath, json);
    }

    private RecordingObject myRecordingObject;
    /// <summary>
    /// Method fired when the user says start recording
    /// </summary>
    public void StartTask()
    {
        //provide audio feedback for recording
        StringReader.Read("started recording");
        //instantiate the recording object
        myRecordingObject = new RecordingObject();
        myRecordingObject.recordingID = "PHD Student ";
        myRecordingObject.applicationName = "_Secondary Task";
        //invoke the method at random interval between 1-5 sec
        int fortimerinterval = rand.Next(1, 6);
        Invoke("ChangeCanvasRandom", fortimerinterval);
        //changed the started recording to true
        startedRecording = true;
    }

    /// <summary>
    /// Method Fired when the user says stop recording
    /// </summary>
    public void StopTask()
    {
        StringReader.Read("stopped recording");
        CancelInvoke();
        startedRecording = false;
        //save the json file
        WriteJson();
    }
}
