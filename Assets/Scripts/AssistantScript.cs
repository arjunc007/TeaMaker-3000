using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantScript : MonoBehaviour {

    Animator anim;
    TextMesh text;

    bool gazed = false;

    public float speechTimeOut = 5;

    float currentTime = 0;
	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        text = GetComponentInChildren<TextMesh>();
        text.text = "Hi There! I'm Chip and we'll make some tea today! Let's Get Going!";
    }
	
	// Update is called once per frame
	void LateUpdate () {

        currentTime += Time.deltaTime;

        if (currentTime > speechTimeOut && !gazed)
            text.text = GameManager.Instance.AvatarText;
 
	}

    void OnGazeEnter()
    {
        anim.SetTrigger("Wave");
        text.text = "TAP to interact or HOLD to place!";
        gazed = true;
    }

    void OnGazeLeave()
    {
        text.text = GameManager.Instance.AvatarText;
        gazed = false;
    }
}
