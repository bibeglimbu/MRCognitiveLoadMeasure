using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.WSA.WebCam;

public class VideoAnnotation: MonoBehaviour
{
    #region VARS

    VideoCapture videoCaptureObject = null;
    private bool isRecording = false;
    private string SaveFileName = "DefaultVideoName.mp4";

    #endregion VARS

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// user has intended to record video
    /// </summary>
    public void StartRecordingVideo()
    {
        isRecording = true;
        VideoCapture.CreateAsync(true, OnVideoCaptureCreated);
    }

    /// <summary>
    /// Start Recording
    /// </summary>
    /// <param name="result"></param>
    void RecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("startrecordingvideo");
        if (result.success)
        {
            string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
            string filename = string.Format("ReactionTimer_{0}.mp4", timeStamp);
            string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
            filepath = filepath.Replace("/", @"\");
            videoCaptureObject.StartRecordingAsync(filepath, OnStartedRecordingVideo);
        }
    }


    /// <summary>
    /// User has intended to stop recording
    /// </summary>
    public void StopRecordingVideo()
    {
        if (isRecording == false)
        {
            Debug.Log("No recording taking place");
            return;
        }
        videoCaptureObject.StopRecordingAsync(OnStoppedRecordingVideo);
    }

    void OnVideoCaptureCreated(VideoCapture videoCapture)
    {

        Debug.Log("onVideoCaptureCreated");
        if (videoCapture != null)
        {
            videoCaptureObject = videoCapture;

            Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
            float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.frameRate = cameraFramerate;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
            videoCaptureObject.StartVideoModeAsync(cameraParameters,
                                                VideoCapture.AudioState.MicAudio,
                                                RecordingVideo);
        }
        else
        {
            Debug.LogError("Failed to create VideoCapture Instance!");
        }
    }


    /// <summary>
    /// update UI or behaviors to enable stopping
    /// </summary>
    /// <param name="result"></param>
    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");

        //gameObject.GetComponent<WEKITVideoAnnotationEditor>().changeColor(Color.green);
        // We will stop the video from recording via other input such as a timer or a tap, etc.
    }



    /// <summary>
    /// recording stopped
    /// </summary>
    /// <param name="result"></param>
    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        isRecording = false;
        videoCaptureObject.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        videoCaptureObject.Dispose();
        videoCaptureObject = null;
    }
}