using UnityEngine;
using System;

public class Mic : MonoBehaviour {

    public IRecordingHandler recordingHandler;

    private AudioClip clip;

    public string micDevice = null;
    public int frequency = 16000;
    public int maxRecordLengthSec = 10;

    private static string audioFilename = "clip";
    private static string audioFilePath = "Assets/"+ audioFilename + ".wav";

    void Start ()
    {
        // bypass SSL
        // TODO: find better alternative
        System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Recording...");
            bool loop = false;
            clip = Microphone.Start(micDevice, loop, maxRecordLengthSec, frequency);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            bool saveSuccess;
            Debug.Log("Trimming and saving recording...");

            // Save the audio file
            int lastTime = Microphone.GetPosition(micDevice);
            Microphone.End(micDevice);
            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);
            float[] clipSamples = new float[lastTime];
            Array.Copy(samples, clipSamples, clipSamples.Length - 1);
            bool stream = false;
            clip = AudioClip.Create("playRecordClip", clipSamples.Length, 1, frequency, stream);
            clip.SetData(clipSamples, 0);

            saveSuccess = SavWav.Save(audioFilename, clip);
            if (!saveSuccess)
            {
                Debug.LogError("Error saving audio clip");
            }

            clip = null;

            recordingHandler.handleRecordingAudio(audioFilePath);           
        }
    }
}
