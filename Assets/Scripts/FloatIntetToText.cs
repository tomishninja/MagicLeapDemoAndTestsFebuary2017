using Assets.LSL4Unity.Scripts;
using Assets.LSL4Unity.Scripts.AbstractInlets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatIntetToText : InletFloatSamples {

    private bool pullSamplesContinuously = false;

    public Text outputGUITextObject = null;

	// Use this for initialization
	void Start () {

    }

    /// <summary>
    /// Checks if the stream is the expeted one
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    protected override bool isTheExpected(LSLStreamInfoWrapper stream)
    {
        // the base implementation just checks for stream name and type
        var predicate = base.isTheExpected(stream);
        // add a more specific description for your stream here specifying hostname etc.
        //predicate &= stream.HostName.Equals("Expected Hostname");
        return predicate;
    }

    /// <summary>
    /// Override this method to implement whatever should happen with the samples...
    /// IMPORTANT: Avoid heavy processing logic within this method, update a state and use
    /// coroutines for more complexe processing tasks to distribute processing time over
    /// several frames
    /// </summary>
    /// <param name="newSample"></param>
    /// <param name="timeStamp"></param>
    protected override void Process(float[] newSample, double timeStamp)
    {
        if (this.outputGUITextObject != null && newSample != null && newSample.Length != 0)
        {
            string output = "";
            for (int i = 0; i < newSample.Length; i++)
            {
                //Generate the output string
                output += newSample[i];
                if (i < newSample.Length - 1)
                {
                    output += ", ";
                }
            }

            // assign it to the graphic
            this.outputGUITextObject.text = output;
        }
        
    }

    protected override void OnStreamAvailable()
    {
        pullSamplesContinuously = true;
    }

    protected override void OnStreamLost()
    {
        pullSamplesContinuously = false;
    }

    private void Update()
    {
        if (pullSamplesContinuously)
            pullSamples();
    }
}
