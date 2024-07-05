using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    
}
public class CMessage
{
    public long msgID = -1;
    public string text = "";
    public List<CAnswer> answers = new List<CAnswer>();
}

public class CAnswer
{
    public long answID = -1;
    public string text = "";
    public long msgID = -1;
    public string action = "";
}

public class CDialogue
{
    List<CMessage> messages = new List<CMessage>();
    long UID = 0;
    CMessage selectedMessage = null;
    CAnswer selectedAnswer = null;

    long getUID()
    {
        UID++;
        return UID;
    }

    CMessage findMsg(long msgID)
    {
        foreach (CMessage msg in messages)
        {
            if (msg.msgID == msgID)
            {
                return msg;
            }
        }

        return null;
    }

    CAnswer findAnsw(long answID)
    {
        foreach (CMessage msg in messages)
        {
            foreach (CAnswer answ in msg.answers)
            {
                if (answ.answID == answID)
                {
                    return answ;
                }
            }
        }

        return null;
    }

    public string selectMessage(long msgID)
    {
        selectedMessage = findMsg(msgID);
        return selectedMessage.text;
    }

    public string selectAnswer(long msgID, long answID)
    {
        selectMessage(msgID);
        selectedAnswer = findAnsw(answID);

        return selectedAnswer.text + " [action: " + selectedAnswer.action + "]";
    }

    public List<CMessage> getMessages()
    {
        return messages;
    }

    public long linkedUID()
    {
        if (selectedMessage != null)
        {
            return selectedAnswer.msgID;
        }

        return 0;
    }
    public void Clear()
    {
        UID = 0;
        messages.Clear();
    }
    public void loadMessage(CMessage msg)
    {
        messages.Add(msg);
        selectedMessage = msg;
    }

    public void loadAnswer(CAnswer answ)
    {
        selectedMessage.answers.Add(answ);
    }

    public List<CAnswer> getAnswers()
    {
        return selectedMessage.answers;
    }

}
