using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using System.Diagnostics;

public class MovementStateManager : MonoBehaviour
{
    public float moveSpeed = 2;
    [HideInInspector] public float originalMoveSpeed;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Vector3 dir;
    CharacterController controller;

    public float groundYOffset;
    public LayerMask groundMask;
    Vector3 spherePosition;

    float gravity = -9.81f;
    Vector3 velocity;

    float xAxis, yAxis;
    public float mouseSense = 1;
    public Transform camFollowPos;

    BaseMoveState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    [HideInInspector] public Animator anim;

    public float interactionRange = 1f; // ���������� ��� ��������������
    public LayerMask doorLayer;
    public LayerMask mainDoorLayer;// ���� "Door"
    public LayerMask lockDoorLayer;
    public LayerMask InteractableObject;
    public TMP_Text interactionText; // ������ �� TMP_Text
    private DoorInteraction currentDoor; // ������� �����, � ������� ���������������

    public LayerMask npcLayer;

    public float invisibleDuration = 20f;
    public float speedDuration = 20f;
    public Material toonMat;

    public GameObject fullTimeBar;
    public Image timeBar; // ��������� Image �� Canvas ����� ���������
    private float timer;
    public TMP_Text potionDurName;

    private bool canUsePotion = true;
    [HideInInspector] public bool sprint;

    public TMP_Text message;

    // Start is called before the first frame update
    void Start()
    {
        originalMoveSpeed = moveSpeed;
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
        toonMat.SetFloat("_Tweak_transparency", 0);
        timer = invisibleDuration;
        timeBar.fillAmount = 1;
        sprint = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove();
        Gravity();

        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        anim.SetFloat("hzInput", hzInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this);

        InterractionWithObjects();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<UIMainMenu>().ActiveMenuPanel();
        }
    }

    private void LateUpdate()       // �������� ������ ������ ���������
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(BaseMoveState state)       // ������������ ��������
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()      // ��������
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
    }

    bool IsGround()     //�������� �� ������������ � �����
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);

        if (Physics.CheckSphere(spherePosition, controller.radius - 0.05f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()      // ������� �������
    {
        if (!IsGround())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void InterractionWithObjects()
    {
        RaycastHit hit;

        // ������� ��� ������ �� ������
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, doorLayer))
        {
            // ���� ������ �� ���� "Door", �� ������� �������� � ����������� ��������������
            DoorInteraction door = hit.collider.GetComponentInChildren<DoorInteraction>();
            interactionText.text = door.doorName;

            // ���� ����� �������� ������� E, �� ��������� ������� �����
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentDoor = door;
            }
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, lockDoorLayer))
        {
            // ���� ������ �� ���� "Door", �� ������� �������� � ����������� ��������������
            DoorInteraction door = hit.collider.GetComponentInChildren<DoorInteraction>();
            interactionText.text = door.doorName;

            // ���� ����� �������� ������� E, �� ��������� ������� �����
            if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<PlayerStats>().isKey == true)
            {
                currentDoor = door;
            }
            else if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<PlayerStats>().isKey == false)
            {
                FindObjectOfType<AudioManager>().Play("Lock Door");
            }
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, mainDoorLayer))
        {
            // ���� ������ �� ���� "Door", �� ������� �������� � ����������� ��������������
            DoorInteraction door = hit.collider.GetComponent<DoorInteraction>();
            interactionText.text = door.mainDoorName;

            if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<PlayerStats>().isMainKey == true)
            {
                door.MainDoorOpen();
            }
            if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<PlayerStats>().isMainKey == false)
            {
                FindObjectOfType<AudioManager>().Play("Lock Door");
            }
            if (gameObject.GetComponent<PlayerStats>().isMainKey == false)
            {
                interactionText.text = "��� ����� �� ������";
            }
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, InteractableObject))
        {
            InteractableScript inter = hit.collider.GetComponent<InteractableScript>();
            interactionText.text = inter.interactableName;

            if (Input.GetKeyDown(KeyCode.E) && inter.interactableEnabled)
            {
                inter.Interact();
            }
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, npcLayer))
        {
            NPCScript npc = hit.collider.GetComponent<NPCScript>();

            if (hit.collider.gameObject.name == "Bernard")
                interactionText.text = npc.npcName;

            else
                interactionText.text = npc.chestName;

            if ((Input.GetKeyDown(KeyCode.E) && npc.isSpeak))
            {
                npc.interactNPC();
            }
            if (Input.GetKeyDown(KeyCode.E) && npc.invisChestIsOpen)
            {
                npc.interactInvisiblePotionChest();
            }
            if (Input.GetKeyDown(KeyCode.E) && npc.speedChestIsOpen)
            {
                npc.interactSpeedPotionChest();
            }
            if (Input.GetKeyDown(KeyCode.E) && npc.keyChestIsOpen)
            {
                npc.interactKey();
            }
        }

        else
        {
            interactionText.text = "";
            currentDoor = null;
        }

        // ���� ������� ����� ����������� � ����� �������� E, �� �������� ����� DoorOpen ��� ������� �����
        if (currentDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            currentDoor.DoorOpen();
        }

        if (Input.GetKeyDown(KeyCode.I) && gameObject.GetComponent<PlayerStats>().invisiblePotion == true && canUsePotion)
        {
            potionDurName.text = "Potion of invisible";
            StartCoroutine(Invisible());
        }

        if (Input.GetKeyDown(KeyCode.U) && gameObject.GetComponent<PlayerStats>().speedPotion == true && canUsePotion)
        {
            potionDurName.text = "Potion of speed";
            StartCoroutine(Speed());
        }
    }

    IEnumerator Invisible()
    {
        if (gameObject.GetComponent<PlayerStats>().potionsOfInvisible > 0 && canUsePotion)
        {
            FindObjectOfType<AudioManager>().Play("Potion");
            canUsePotion = false;
            gameObject.GetComponent<PlayerStats>().potionsOfInvisible--;

            toonMat.SetFloat("_Tweak_transparency", -0.6f);
            ChangeRadiusOfEnemies(0.1f);
            fullTimeBar.gameObject.SetActive(true);

            while (timer > 0)
            {
                // ��������� ����� �������
                UpdateTimeBar(invisibleDuration);

                // ��������� ������
                timer -= Time.deltaTime;

                yield return null;
            }

            fullTimeBar.gameObject.SetActive(false);
            toonMat.SetFloat("_Tweak_transparency", 0);
            ChangeRadiusOfEnemies(FindObjectOfType<EnemyFOV>().originalRadius);

            timer = invisibleDuration;
            canUsePotion = true;
        }
        else
        {
            StartCoroutine(EnterMessage("������� ���������� ������ ����������"));
        }
    }

    IEnumerator Speed()
    {
        if (gameObject.GetComponent<PlayerStats>().potionsOfSpeed > 0 && canUsePotion)
        {
            FindObjectOfType<AudioManager>().Play("Potion");
            canUsePotion = false;
            gameObject.GetComponent<PlayerStats>().potionsOfSpeed--;

            fullTimeBar.gameObject.SetActive(true);
            sprint = true;

            while (timer > 0)
            {
                // ��������� ����� �������
                UpdateTimeBar(speedDuration);

                // ��������� ������
                timer -= Time.deltaTime;

                yield return null;
            }

            sprint = false;
            fullTimeBar.gameObject.SetActive(false);

            timer = speedDuration;
            canUsePotion = true;
        }
        else
        {
            StartCoroutine(EnterMessage("������� ���������� ������ ����������"));
        }
    }

    void UpdateTimeBar(float duration)
    {
        float fillAmount = timer / duration;
        timeBar.fillAmount = fillAmount;
    }

    void ChangeRadiusOfEnemies(float newRadius)
    {
        EnemyFOV[] enemyFOVScripts = FindObjectsOfType<EnemyFOV>();

        foreach (EnemyFOV enemy in enemyFOVScripts)
        {
            enemy.radius = newRadius;
        }
    }

    public IEnumerator EnterMessage(string msg)
    {
        message.gameObject.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(5);
        message.gameObject.SetActive(false);
    }
}
