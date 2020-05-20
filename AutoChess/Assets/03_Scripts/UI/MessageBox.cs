using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    GameObject messageObj;
    Text message;
    float messageDuration;
    bool isMessageOn;
    public enum MessageType
    {
        LEVEL_UP,
        NOT_ENOUGH_GOLD,
        NOT_ENOUGH_INVEN
    }
    private void Awake()
    {
        messageObj = transform.Find("Message").gameObject;
        message = messageObj.transform.Find("Text").GetComponent<Text>();
        messageDuration = 1.5f;
        isMessageOn = false;
    }

    public void OnMessageBox(MessageType type)
    {
        string colorText1 = default;
        string colorText2 = default;
        string text = default;

        switch (type)
        {
            case MessageType.LEVEL_UP:
                colorText1 = "<color=#8EEC39FF>레벨 업</color>";
                colorText2 = "<color=#8EEC39FF>배치 수용량+1</color>";
                text = "캐릭터 " + colorText1 + "! 히어로 " + colorText2 + "!";
                break;
            case MessageType.NOT_ENOUGH_GOLD:
                colorText1 = "<color=#8EEC39FF>골드</color>";
                text = colorText1 + "가 모자랍니다!";
                break;
            case MessageType.NOT_ENOUGH_INVEN:
                colorText1 = "<color=#8EEC39FF>인벤토리</color>";
                text = colorText1 + "에 자리가 없습니다!";
                break;
        }

        if(isMessageOn == false)
        {
            messageObj.SetActive(true);
            message.text = text;
            isMessageOn = true;

            StartCoroutine(OffMessageBox());
        }
    }
    IEnumerator OffMessageBox()
    {
        yield return new WaitForSeconds(messageDuration);
        message.text = default;
        isMessageOn = false;
        messageObj.SetActive(false);
    }
}
