using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public string doorName = "Дверь";
    public string mainDoorName = "Двойная дверь";// Задайте название двери в инспекторе
    public Animator doorAnimator; // Ссылка на аниматор двери


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
        // Получаем индекс текущего активного уровня
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
    //    // Отписываемся от события загрузки сцены, чтобы избежать утечек памяти
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //// Метод, который будет вызван после загрузки новой сцены
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // Здесь можно выполнить действия после загрузки сцены
    //    // Например, загрузить статы игрока

    //}
}
