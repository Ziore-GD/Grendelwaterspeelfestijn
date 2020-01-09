using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Announcer : MonoBehaviour {
    private Text _announceTxt;
    private GameObject panel;
    private static Announcer _announcer;
    public static Announcer Instance {
        get {
            if (_announcer == null) _announcer = FindObjectOfType<Announcer> ();
            return _announcer;
        }
    }

    void Awake () {
        _announceTxt = GetComponent<Text> ();
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(false);
        _announceTxt.text = "";
    }
    private IEnumerator TextVisibility () {
        yield return new WaitForSeconds (3);
        _announceTxt.text = "";
        panel.SetActive(false);
        yield return null;
    }
    public void Log (string txt) {
        _announceTxt.text = txt;
        panel.SetActive(true);
        StartCoroutine (TextVisibility ());
    }
}