using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TriggerMusic : MonoBehaviour {

    public GameObject triggerController;
    public GameObject[] sprites;

    AudioSource audioSource;
    bool isPlaying = true;

	private void Start()
	{
        audioSource = GetComponent<AudioSource>();
        foreach(GameObject s in sprites)
            s.GetComponent<SpriteRenderer>().color = Color.green;
	}

    public void switchMusic(object o, InteractableObjectEventArgs ioea)
    {
        if (ioea.interactingObject != triggerController) return;

        if (!isPlaying)
        {
            foreach(GameObject s in sprites)
                s.GetComponent<SpriteRenderer>().color = Color.green;
            audioSource.volume = 1.0f;
            isPlaying = true;
        }
        else
        {
            foreach(GameObject s in sprites)
                s.GetComponent<SpriteRenderer>().color = Color.red;
            audioSource.volume = 0.0f;
            isPlaying = false;
        }

    }

}
