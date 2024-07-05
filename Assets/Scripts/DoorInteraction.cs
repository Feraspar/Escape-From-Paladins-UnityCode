using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public string doorName = "�����";
    public string mainDoorName = "������� �����";// ������� �������� ����� � ����������
    public Animator doorAnimator; // ������ �� �������� �����


    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }
    public void DoorOpen()
    {
        if (doorAnimator.GetBool("Open") == false)
        {
            doorAnimator.SetBool("Open", true);
            FindObjectOfType<AudioManager>().Play("Open Door");
        }
        else
        {
            doorAnimator.SetBool("Open", false);
            FindObjectOfType<AudioManager>().Play("Close Door");
        }
    }

    public void MainDoorOpen()
    {
        FindObjectOfType<AudioManager>().Play("Open Door");
        FindObjectOfType<PlayerStats>().isMainKey = false;
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        FindObjectOfType<PlayerStats>().SavePlayerStats();
        // �������� ������ �������� ��������� ������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(FindObjectOfType<UIMainMenu>().LoadAndActivateScene(currentSceneIndex + 1));
        }
        else
        {
            FindObjectOfType<UIMainMenu>().ActiveWinPanel();
        }
    }

    //private void OnEnable()
    //{
    //    FindObjectOfType<PlayerStats>().LoadPlayerStats();
    //}

    //private void OnDisable()
    //{
    //    // ������������ �� ������� �������� �����, ����� �������� ������ ������
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //// �����, ������� ����� ������ ����� �������� ����� �����
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // ����� ����� ��������� �������� ����� �������� �����
    //    // ��������, ��������� ����� ������

    //}
}
