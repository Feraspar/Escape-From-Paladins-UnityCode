using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public delegate void action();
public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueWindow;
    public GameObject answers;
    public TMP_Text message;
    public TMP_Text answer;
    public MovementStateManager player;

    Dictionary<string, action> actions = new Dictionary<string, action>();

    CDialogue dialogue = new CDialogue();

    public Stats stats;

    public void loadDialogue(Object xmlFile)
    {
        dialogue.Clear();
        actions.Clear();
        actions.Add("Завершить диалог", dialogueEnd);
        actions.Add("none", null);
        actions.Add("Взять зелье невидимости", null);
        actions.Add("Взять зелье скорости", null);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.ToString());
        XmlNode messages = xmlDoc.SelectSingleNode("//Messages");
        XmlNodeList messageNodes = xmlDoc.SelectNodes("//Messages/Message");

        foreach (XmlNode messageNode in messageNodes)
        {
            CMessage msg = new CMessage();
            msg.text = messageNode.ChildNodes[0].InnerText;
            msg.msgID = long.Parse(messageNode.Attributes["uid"].Value);
            dialogue.loadMessage(msg);

            foreach (XmlNode answerNode in messageNode.ChildNodes[1].ChildNodes)
            {
                CAnswer answ = new CAnswer();
                answ.answID = long.Parse(answerNode.Attributes["auid"].Value);
                answ.msgID = long.Parse(answerNode.Attributes["muid"].Value);
                answ.action = answerNode.Attributes["action"].Value;
                answ.text = answerNode.InnerText;
                dialogue.loadAnswer(answ);
            }
        }
        showMessage(dialogue.getMessages()[0].msgID, "none");
        dialogueWindow.SetActive(true);
    }

    public void showMessage(long uid, string act)
    {
        actions[act]?.Invoke();
        if (uid == -1) return;
        foreach (Transform child in answers.transform)
        {
            Destroy(child.gameObject);
        }

        message.text = dialogue.selectMessage(uid);

        foreach (CAnswer ans in dialogue.getAnswers())
        {
            TMP_Text txt = Instantiate<TMP_Text>(answer);
            txt.text = ans.text;

            txt.GetComponent<Button>().onClick.AddListener(delegate { showMessage(ans.msgID, ans.action); });
            txt.transform.SetParent(answers.transform);
        }
    }

    public void dialogueEnd()
    {
        dialogueWindow.SetActive(false);
        player.mouseSense = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void setAction(string name, action act)
    {
        actions[name] = act;
    }
}
