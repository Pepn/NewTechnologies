using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AudioSynch : MonoBehaviour 
{
    public Transform rightHand;

    public AudioSource master;
    public AudioSource[] slaves;

    public int originalBPM;
    public float stopWaveThreshold;
    public float startWaveThreshold;

    private float prevHitTime;
    private float currentHitTime;
    private Vector3 prevHandPos;
    private Vector3 currentHandPos;

    private float currentPitch;
    private bool isWaving;
	private VRTK_ControllerEvents controlEvents;
	private float prevBPM;

    private int counter;

	private void Start()
	{
        prevHandPos = rightHand.position;
        currentHandPos = rightHand.position;

        currentPitch = 1.0f;
        isWaving = false;
        counter = 0;

		prevBPM = 120;
		controlEvents = rightHand.GetComponent<VRTK_ControllerEvents> ();
	}

	private void Update()
	{
		if (controlEvents.touchpadPressed)
        	UpdateBPM();

        master.pitch = currentPitch;

        foreach(AudioSource audioSource in slaves)
        {
            audioSource.timeSamples = master.timeSamples;
            audioSource.pitch = master.pitch;
        }

	}

    float prevSpeed = 0.0f;
    float speed = 0.0f;

    private void UpdateBPM()
    {
        prevHandPos = currentHandPos;
        currentHandPos = rightHand.position;

        prevSpeed = speed;
        speed = Mathf.Abs((prevHandPos - currentHandPos).magnitude);

		if (isWaving) {
			if (speed <= stopWaveThreshold) {
				prevHitTime = currentHitTime;
				currentHitTime = Time.time;
				float timeDiff = currentHitTime - prevHitTime;

				float BPM = 60.0f / timeDiff;
				if (BPM > 250) 
				{
					BPM = prevBPM;
					isWaving = false;
					return;
				}

				//Debug.Log ("Beat: " + counter++ + ", BPM " + BPM);
				//Debug.Log ("STOP: ");
				currentPitch = CalculatePitch (timeDiff);

				isWaving = false;
				return;
			}
		}
        else
        {
            if (speed > startWaveThreshold)
            {
				//Debug.Log ("START WAVE" + counter++);
                isWaving = true;
                return;
            }
        }
    }

    private float CalculatePitch(float timeDiff)
    {
        return 60.0f / (originalBPM * timeDiff);
    }


    private float max = 0.001f;
    private bool debugIsWaving = false;

    private void LogMaxSpeed()
    {
        if (!debugIsWaving)
        {
            if (speed > prevSpeed) debugIsWaving = true;
        }

        if (debugIsWaving)
        {
            if (speed < prevSpeed)
            {
                Debug.Log("MAX SPEED(" + counter + ") = " + prevSpeed);
                debugIsWaving = false; 
            }
        }

    }


}
