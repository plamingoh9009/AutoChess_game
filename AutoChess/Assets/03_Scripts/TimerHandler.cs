using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    int _timer;
    UnityEngine.UI.Text _uiText;
    private void Awake()
    {
        _timer = 30;
        _uiText = GameObject.Find("Count").GetComponent<UnityEngine.UI.Text>();
    }
    private void Start()
    {
        StartCoroutine(PastOneSecond());
    }

    IEnumerator PastOneSecond()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _timer--;
            _uiText.text = _timer.ToString();
        }
    }
}
