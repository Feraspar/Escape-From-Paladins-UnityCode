using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public GameObject panelWin;
    public GameObject panelLose;
    public GameObject panelMenu;

    public Image InvisiblePotion;
    public Image SpeedPotion;

    public TMP_Text calcInvis;
    public TMP_Text calcSpeed;

    private void Update()
    {
        if (calcInvis.text != "0")
        {
            calcInvis.text = FindObjectOfType<PlayerStats>().potionsOfInvisible.ToString();            
        }

        if (calcSpeed.text != "0")
        {
            calcSpeed.text = FindObjectOfType<PlayerStats>().potionsOfSpeed.ToString();
        }

        if (calcInvis.text == "0")
        {
            DisableInvisIcon();
        }

        if (calcSpeed.text == "0")
        {
            DisableSpeedIcon();
        }
    }
    public void OnClick()
    {
        FindObjectOfType<AudioManager>().Play("OnClick");
    }

    public void OnEnter()
    {
        FindObjectOfType<AudioManager>().Play("OnEnter");
    }

    public void ActiveMenuPanel()
    {
        panelMenu.SetActive(true);
        FindObjectOfType<MovementStateManager>().mouseSense = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAndActivateScene(2));
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
        panelWin.SetActive(false);
        panelLose.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        panelMenu.SetActive(false);
        FindObjectOfType<MovementStateManager>().mouseSense = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ActiveLosePanel()
    {
        panelLose.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ActiveWinPanel()
    {
        panelWin.SetActive(true);
        FindObjectOfType<PlayerStats>().characterController.enabled = false;
        FindObjectOfType<PlayerStats>().collider.enabled = false;
        FindObjectOfType<MovementStateManager>().mouseSense = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ActiveInvisIcon()
    {
        InvisiblePotion.gameObject.SetActive(true);
        calcInvis.text = FindObjectOfType<PlayerStats>().potionsOfInvisible.ToString();
    }

    public void ActiveSpeedIcon()
    {
        SpeedPotion.gameObject.SetActive(true);
        calcSpeed.text = FindObjectOfType<PlayerStats>().potionsOfSpeed.ToString();
    }

    public void DisableInvisIcon()
    {
        InvisiblePotion.gameObject.SetActive(false);
        calcInvis.text = "";
    }

    public void DisableSpeedIcon()
    {
        SpeedPotion.gameObject.SetActive(false);
        calcSpeed.text = "";
    }

    public IEnumerator LoadAndActivateScene(int lvl)
    {
        // Загрузка сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            yield return null;
        }
        // Найдите объект в новой сцене и вызовите метод
        Scene loadedScene = SceneManager.GetSceneByName("LoadingScreen");
        GameObject[] sceneObjects = loadedScene.GetRootGameObjects();
        foreach (GameObject obj in sceneObjects)
        {
            Debug.Log(123);
            Loading script = obj.GetComponent<Loading>();
            if (script != null)
            {
                script.LoadScene(lvl);
                yield return null;
            }
        }

        SceneManager.UnloadSceneAsync("LoadingScreen");
    }
}
