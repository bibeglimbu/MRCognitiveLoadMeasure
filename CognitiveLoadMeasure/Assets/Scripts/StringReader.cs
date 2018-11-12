using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringReader : MonoBehaviour {

    private static TextToSpeech textToSpeech;

	// Use this for initialization
	void Start () {
        textToSpeech = GameObject.Find("Managers").GetComponent<TextToSpeech>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // <summary>
    /// Read the text
    /// </summary>
    /// <param name="text">Text to be spoken.</param>
    public static void Read(string text)
    {
        // Don't talk empty and don't talk over.
        if (!string.IsNullOrEmpty(text) && !textToSpeech.IsSpeaking())
            textToSpeech.StartSpeaking(text);
    }
}
