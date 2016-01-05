using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    public AudioClip openSound;
    public AudioClip closeSound;
    public float TimeBeforeClosing = 2.0f;

    bool open = false;
    AudioSource audioSource;
    Animator animator;

    float m_closeTimer = 0;
    bool m_closeCountdown = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Open()
    {
        if (open)
            return;

        animator.SetTrigger("open");
        audioSource.clip = openSound;
        audioSource.Play();

        open = true;
    }

    void Close()
    {
        if (!open)
            return;

        animator.SetTrigger("close");
        audioSource.clip = closeSound;
        audioSource.PlayDelayed(1.9f);

        open = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(m_closeCountdown)
        {
            m_closeTimer += Time.deltaTime;
            if(m_closeTimer >= TimeBeforeClosing)
            {
                StopCloseCountdown();
                Close();
            }
        }

	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            StopCloseCountdown();
            Open();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            StartCloseCountdown();
        }
    }

    void StartCloseCountdown()
    {
        m_closeTimer = 0;
        m_closeCountdown = true;
    }

    void StopCloseCountdown()
    {
        m_closeCountdown = false;
    }
}
