using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommand : MonoBehaviour {
    LearningHubControl LHC = new LearningHubControl();
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    // Use this for initialization
    void Start () {
        keywords.Add("Start Recording", () =>
        {
            GameObject.Find("Managers").GetComponent<LearningHubControl>().sendMessage("<START RECORDING>");
            GameObject.Find("Managers").GetComponent<VideoAnnotation>().StartRecordingVideo();
            GameObject.Find("Canvas").GetComponent<CanvasModifier>().StartTask();
            Debug.Log("Started Recording");

        });

        keywords.Add("Stop Recording", () =>
        {
            GameObject.Find("Managers").GetComponent<LearningHubControl>().sendMessage("<STOP RECORDING>");
            GameObject.Find("Managers").GetComponent<VideoAnnotation>().StopRecordingVideo();
            GameObject.Find("Canvas").GetComponent<CanvasModifier>().StopTask();
            Debug.Log("Stopped Recording");
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
