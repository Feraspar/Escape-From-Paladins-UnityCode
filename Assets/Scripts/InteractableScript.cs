using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public string interactableName = "Светильник";
    public Animator interactableAnimator;
    public bool interactableEnabled;

    private void Start()
    {
        interactableAnimator = GetComponent<Animator>();
        interactableEnabled = true;
    }

    public void Interact()
    {
        FindObjectOfType<AudioManager>().Play("Crash");
        interactableEnabled = false;
        interactableAnimator.SetBool("Breake", true);

        FindObjectOfType<EnemyScript>().AI_Enemy = EnemyScript.AI_State.Check;
    }
}
