using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Next Scene")]
    [SerializeField] private string nextScene;
    [SerializeField] private float time;
    private bool playerInRange;
    private bool waitForNextScene;
    public bool enterWhenExitDialogue;
    public bool exitDialogueMode;

    private static NextScene instance_;
    // Start is called before the first frame update
    private void Awake()
    {
        instance_ = this;
    }

    public static NextScene GetInstance()
    {
        return instance_;
    }
    void Start()
    {
        playerInRange = false;
        waitForNextScene = false;
        exitDialogueMode = false;
        visualCue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !waitForNextScene)
        {
            visualCue.SetActive(true);
            if ((Input.GetButtonDown("Dialogue") && !enterWhenExitDialogue) || exitDialogueMode)
            {
                StartCoroutine(EnterNextScene());
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    IEnumerator EnterNextScene()
    {
        waitForNextScene = true;
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nextScene);
    }
}
