using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicControl : MonoBehaviour
{
    public bool canContinue = true, transition = false;
    public Transform[] states;
    int actualState;
    float startTransitionTime;


    private void Start()
    {
        transform.position = states[0].position;
        actualState = 0;
    }
    
    private void Update()
    {
        Debug.Log("actual state" + actualState);
        if (canContinue && Input.GetButtonDown("Fire1"))
        {
            startTransitionTime = Time.time;
            StartCoroutine(cooldown());
        }
        else if (transition) 
        {
            Debug.Log(states.Length);
            if (states.Length > actualState+1) {
                Vector3 newpos = new Vector3(Mathf.Lerp(states[actualState].position.x, states[actualState+1].position.x, Time.time - startTransitionTime) , transform.position.y, transform.position.z);
                transform.position = newpos;
            }
            else
            {
                SceneManager.LoadScene("_Final");
            }
        }
    }

    public IEnumerator cooldown()
    {
        canContinue = false;
        transition = true;

        yield return new WaitForSeconds(2);

        canContinue = true;
        transition = false;
        actualState++;
    }
}
