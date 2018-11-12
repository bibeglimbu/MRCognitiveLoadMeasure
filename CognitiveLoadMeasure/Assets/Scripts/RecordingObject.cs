using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class RecordingObject{

    public string recordingID;
    public string applicationName;
    public List<string> frames = new List<string>();

    public RecordingObject()
    {

    }

}
