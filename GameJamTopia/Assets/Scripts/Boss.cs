using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody bossRb;
    public Animator bossAnimator;
    public GameObject victoryTrigger;

    private bool isActive = true;
    private bool checkDistance = false;

    public AudioClip audioBoss;
    public AudioSource musicAudioSource;
    public AudioClip endMusic;
    private float originalVolumeMusic;

    public GameObject[] toActive;

    void OnTriggerEnter(Collider collider)
    {
        if(isActive && collider.tag == "Player")
        {
            isActive = false;
            bossAnimator.enabled = true;
            this.GetComponent<AudioSource>().PlayOneShot(audioBoss);
            PlayerController.instance.GetComponent<CameraShake>().shakeDuration = 1f;
            originalVolumeMusic = musicAudioSource.volume;
            musicAudioSource.volume = 0f;
            StartCoroutine(StartBoss());
            victoryTrigger.SetActive(true);
        }
    }

    private IEnumerator StartBoss()
    {
        yield return new WaitForSeconds(1.5f);        
        foreach(GameObject active in toActive)
        {
            active.SetActive(true);
        }
        PlayerController.instance.GetComponent<CameraShake>().shakeDuration = 4f;
        bossRb.velocity = new Vector3(-3f, 0, 0);
        checkDistance = true;
        yield return new WaitForSeconds(1.5f);
        musicAudioSource.Stop();
        musicAudioSource.PlayOneShot(endMusic);
        musicAudioSource.volume = originalVolumeMusic;
    }

    void FixedUpdate()
    {
        if(checkDistance)
        {
            float distance = Vector3.Distance(PlayerController.instance.transform.position, bossRb.transform.position) - 9f;
            if(distance > 500f)
            {
                bossRb.velocity = new Vector3(-100f, 0, 0);
            }
            else if(distance > 200f)
            {
                bossRb.velocity = new Vector3(-20f, 0, 0);
            }
            else if(distance > 100f)
            {
                bossRb.velocity = new Vector3(-15f, 0, 0);
            }
            else if(distance > 50f)
            {
                bossRb.velocity = new Vector3(-8f, 0, 0);
            }
            else if(distance > 25f)
            {
                bossRb.velocity = new Vector3(-4f, 0, 0);
            }
            else{
                bossRb.velocity = new Vector3(-3f, 0, 0);
            }
            
        }
    }
}
