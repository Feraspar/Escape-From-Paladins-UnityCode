using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    private bool gameOver;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Collider collider;

    public bool isKey = false;
    public bool isMainKey = false;
    public bool invisiblePotion = false;
    public bool speedPotion = false;
    public int potionsOfInvisible = 1;
    public int potionsOfSpeed = 2;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameOver = true;
        }
    }

    private void Start()
    {
        Debug.Log(1);
        gameOver = false;
        characterController = GetComponent<CharacterController>();
        collider = GetComponent<CapsuleCollider>();
        //isKey = false;
        //isMainKey = false;
        //invisiblePotion = false;
        //speedPotion = false;
        //potionsOfInvisible = 1;
        //potionsOfSpeed = 2;
        LoadPlayerStats();

        if (invisiblePotion == true)
        {
            FindObjectOfType<UIMainMenu>().ActiveInvisIcon();
        }
        if (speedPotion == true)
        {
            FindObjectOfType<UIMainMenu>().ActiveSpeedIcon();
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            characterController.enabled = false;
            collider.enabled = false;
            gameObject.GetComponent<MovementStateManager>().anim.SetBool("Walking", false);
            gameObject.GetComponent<MovementStateManager>().anim.SetBool("Death", true);
            gameObject.GetComponent<MovementStateManager>().mouseSense = 0;
            FindObjectOfType<UIMainMenu>().ActiveLosePanel();
        }
    }

    public void SavePlayerStats()
    {
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("PotionsOfInvisible", potionsOfInvisible);
        PlayerPrefs.SetInt("PotionsOfSpeed", potionsOfSpeed);

        PlayerPrefs.SetInt("IsKey", isKey ? 1 : 0);
        PlayerPrefs.SetInt("IsMainKey", isMainKey ? 1 : 0);
        PlayerPrefs.SetInt("InvisiblePotion", invisiblePotion ? 1 : 0);
        PlayerPrefs.SetInt("SpeedPotion", speedPotion ? 1 : 0);

    }

    // ћетод дл€ загрузки статов при старте нового уровн€
    public void LoadPlayerStats()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ResetPlayerStats();
        }
        else if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            health = PlayerPrefs.GetInt("Health", 100);
            potionsOfInvisible = PlayerPrefs.GetInt("PotionsOfInvisible", 1);
            potionsOfSpeed = PlayerPrefs.GetInt("PotionsOfSpeed", 2);
            isKey = PlayerPrefs.GetInt("IsKey", 0) == 1;
            isMainKey = PlayerPrefs.GetInt("IsMainKey", 0) == 1;
            invisiblePotion = PlayerPrefs.GetInt("InvisiblePotion", 0) == 1;
            speedPotion = PlayerPrefs.GetInt("SpeedPotion", 0) == 1;
        }
    }

    private void ResetPlayerStats()
    {
        health = 100;
        potionsOfInvisible = 1;
        potionsOfSpeed = 2;
        isKey = false;
        isMainKey = false;
        invisiblePotion = false;
        speedPotion = false;
    }
}
