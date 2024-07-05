using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GrannyMessageCol : MonoBehaviour
{
    private bool isEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player") && isEnter)
        {
            UnityEngine.Debug.Log("Зашли");
            isEnter = false;
            StartCoroutine(FindObjectOfType<MovementStateManager>().EnterMessage("Я слишком стар, чтобы исползовать оружие, но может быть в сундуке найдётся что-то полезное."));
        }
    }

    private void Start()
    {
        isEnter = true;
    }
}
