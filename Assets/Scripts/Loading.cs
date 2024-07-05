using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private readonly string[] Hints = new string[] //��������� �� ����� ��������
    {
        "�� ����� �������� � ����� ������� ������, ���� �� �����.",
        "���������� ������� �� ����� �������.",
        "���� �������� ��� ������, �� ��������� ����� ��� �����.",
        "��������� �� ������ ��� �� ����� ��� �� ���������",
        "� ������ ������ ���� ���� ���� �� �����"
    };
    public TMP_Text ProgressText, HintText, LoadText;
    public Image ProgressImage;
    public void LoadScene(int lvl)
    {
        Debug.Log(123);
        StartCoroutine(LoadingCurrentScene(lvl));
    }

    IEnumerator LoadingCurrentScene(int lvl)
    {
        HintText.text = Hints[Random.Range(0, Hints.Length)];
        AsyncOperation Operation = SceneManager.LoadSceneAsync(lvl);
        Operation.allowSceneActivation = false;
        while (!Operation.isDone)
        {
            float Progress = Operation.progress;
            ProgressText.text = $"{(Progress * 100).ToString("0")}%";
            ProgressImage.fillAmount = Progress;
            if (Operation.progress >= 0.9f)
            {
                LoadText.text = "��� ����������� ������� ����� �������";

                while (!Input.anyKeyDown)
                {
                    yield return null;
                }
                Operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
