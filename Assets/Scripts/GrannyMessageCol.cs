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
            UnityEngine.Debug.Log("�����");
            isEnter = false;
            StartCoroutine(FindObjectOfType<MovementStateManager>().EnterMessage("� ������� ����, ����� ����������� ������, �� ����� ���� � ������� ������� ���-�� ��������."));
        }
    }

    private void Start()
    {
        isEnter = true;
    }
}
