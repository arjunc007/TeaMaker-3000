using UnityEngine;
using System.Collections;
/// <summary>
/// Conveys the set of states available on an Interactible.
/// In the inspector check the states to display on each Interactible.
/// </summary>
[System.Serializable]
public class InteractibleParameters
{
    public bool Scrollable = true;
    public bool Placeable = true;
    public bool Movable = true;
}

/// <summary>
/// The Interactible class flags a Game Object as being "Interactible".
/// Determines what happens when an Interactible is being gazed at.
/// </summary>
public class Interactible : MonoBehaviour
{
    public InteractibleParameters InteractibleParameters;
    public string target;
    public float animSpeed = 5.0f;

    [Tooltip("Audio clip to play when interacting with this hologram.")]
    public AudioClip TargetFeedbackSound;
    private AudioSource audioSource;

    private GameObject halo;

    private bool isAnimating = false;
    private bool returning = false;
    Vector3 targetPos;
    Vector3 startPos;
    float currentTime = 0;
    float animTime = 0;

    Animator anim;

    void Start()
    {
        halo = transform.Find("Halo").gameObject;
        halo.SetActive(false);
        // Add a BoxCollider if the interactible does not contain one.
        Collider collider = GetComponentInChildren<Collider>();
        if (collider == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        EnableAudioHapticFeedback();

        anim = GetComponent<Animator>();
    }

    private void EnableAudioHapticFeedback()
    {
        // If this hologram has an audio clip, add an AudioSource with this clip.
        if (TargetFeedbackSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = TargetFeedbackSound;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
        }
    }

    void LateUpdate()
    {
        Debug.ClearDeveloperConsole();
    }

    private void Update()
    {
        if(isAnimating)
        {
            PlayAnimation();
        }
    }

    void GazeEntered()
    {
        halo.SetActive(true);
    }

    void GazeExited()
    {
        halo.SetActive(false);
    }

    void PlayAnimation()
    { 
        //Check whether to move object before animating
        if (InteractibleParameters.Movable)
        {
            currentTime += Time.deltaTime;


            if (!returning)
                gameObject.transform.position = Vector3.Lerp(startPos, targetPos, currentTime / animTime);
            else
                gameObject.transform.position = Vector3.Lerp(targetPos, startPos, currentTime / animTime);

            if (gameObject.transform.position == targetPos)
            {
                anim.SetTrigger("Pour");

                StartCoroutine(PauseGame());
                //while(anim.GetCurrentAnimatorStateInfo(0).IsName("Animating"))
                //{ }

                if(!returning)
                    returning = true;
                currentTime = 0;
            }

            if (gameObject.transform.position == startPos && returning)
            {
                returning = false;
                isAnimating = false;
            }
        }
        else
        {
            anim.SetTrigger("Pour");
            while (GetComponent<Animation>().isPlaying) ;
            isAnimating = false;
        }

    }

    void StartAnimation()
    {
        isAnimating = true;
        currentTime = 0;

        if (InteractibleParameters.Movable)
        {
            returning = false;

            startPos = transform.position;

            targetPos = GameManager.Instance.SelectedGameObject.GetComponent<Collider>().ClosestPoint(startPos);
            targetPos.y = GameManager.Instance.SelectedGameObject.GetComponent<Collider>().bounds.max.y + 0.05f;

            animTime = Vector3.Distance(startPos, targetPos) / animSpeed;
        }
    }

    IEnumerator PauseGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            Debug.Log("Resume");
        }
    }
}