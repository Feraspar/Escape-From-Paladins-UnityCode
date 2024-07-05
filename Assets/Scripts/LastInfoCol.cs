using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LastInfoCol : MonoBehaviour
{
    private bool isEnter;
    public TMP_Text message;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player") && isEnter)
        {
            isEnter = false;
            StartCoroutine(EnterMessage("Этот стражник не даст мне выйти, нужно его как-то отвлечь."));
        }
    }

    private void Start()
    {
        isEnter = true;
    }

    public IEnumerator EnterMessage(string msg)
    {
        message.gameObject.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(5);
        message.gameObject.SetActive(false);
    }
}
