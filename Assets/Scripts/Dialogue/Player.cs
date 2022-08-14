using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider2D;
    public Animator anim;
    public float moveSpeed;
    public AudioSource runAudio;

    // 水平运动比例
    private float moveX;
    private float moveY;
    private Direction dir;
    private DialogueInputManager dialogueInputManager;
    private DialogueControl dialogueControl;



    // 获取组件
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        dialogueInputManager = GetComponent<DialogueInputManager>();
        dialogueControl = DialogueManager.instance.dialogueControl;
    }

    void FixedUpdate()
    {
        if (PauseControl.gameIsPaused || dialogueControl.IsInDialogue) return;

        switch (dir)
        {
            case Direction.Idle:
                anim.speed = 0;
                StopAudio(runAudio);
                rb.velocity = Vector2.zero;
                break;
            case Direction.Up:
                anim.speed = 1;
                PlayAudio(runAudio);
                anim.CrossFade("Up", 0);
                rb.velocity = Vector2.up * moveSpeed;
                break;
            case Direction.Down:
                anim.speed = 1;
                PlayAudio(runAudio);
                anim.CrossFade("Down", 0);
                rb.velocity = Vector2.down * moveSpeed;
                break;
            case Direction.Left:
                anim.speed = 1;
                PlayAudio(runAudio);
                anim.CrossFade("Left", 0);
                rb.velocity = Vector2.left * moveSpeed;
                break;
            case Direction.Right:
                anim.speed = 1;
                PlayAudio(runAudio);
                anim.CrossFade("Right", 0);
                rb.velocity = Vector2.right * moveSpeed;
                break;
        }
    }

    void Update()
    {
        if (PauseControl.gameIsPaused) return;
        if (dialogueInputManager == null)
        {
            return;
        }

        dir = dialogueInputManager.GetDirection();
    }

    void PlayAudio(AudioSource t)
    {
        if (t != null && !t.isPlaying)
        {
            t.Play();
        }
    }

    void StopAudio(AudioSource t)
    {
        if (t != null && t.isPlaying)
        {
            t.Stop();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bed")    
        {
            other.gameObject.GetComponent<Mark>().ConnectPlayer(true);
            dialogueControl.CurrentReadyDialogue = "bed";
        }
        if (other.gameObject.tag == "Statue")    
        {
            other.gameObject.GetComponent<Mark>().ConnectPlayer(true);
            dialogueControl.CurrentReadyDialogue = "statue";
        }
        if (other.gameObject.tag == "Mirror")    
        {
            if (dialogueControl.CanMirrorDialogueShow)
            {
                other.gameObject.GetComponent<Mark>().ConnectPlayer(true);
                dialogueControl.CurrentReadyDialogue = "mirror";
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bed")    
        {
            other.gameObject.GetComponent<Mark>().ConnectPlayer(false);
            dialogueControl.CurrentReadyDialogue = "";
            dialogueControl.HasShowDialogue = false;
        }
        if (other.gameObject.tag == "Statue")    
        {
            other.gameObject.GetComponent<Mark>().ConnectPlayer(false);
            dialogueControl.CurrentReadyDialogue = "";
            dialogueControl.HasShowDialogue = false;
        }
        if (other.gameObject.tag == "Mirror")    
        {
            if (dialogueControl.CanMirrorDialogueShow)
            {
                other.gameObject.GetComponent<Mark>().ConnectPlayer(false);
                dialogueControl.CurrentReadyDialogue = "";
                dialogueControl.HasShowDialogue = false;
            }
        }
    }
}
