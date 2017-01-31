using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdFetcher : MonoBehaviour {

    [SerializeField]
    string url = "http://wolfewylie.com/cgi-bin/jeopardy.py?callback=?";
    // Use this for initialization
    void Start () {
        DummyPostRequest("japan");
	}
	    
	// Update is called once per frame
	void Update () {
		
	}

    void DummyPostRequest(string country) {
        WWWForm form = new WWWForm();
        form.AddField("question", country);

        WWW www = new WWW(url, form);
        
        StartCoroutine(WaitFor(www));
    }

    IEnumerator WaitFor(WWW www) {
        yield return www;

        if (www.error == null)
        {
            QuizDetails[] details = JsonUtility.FromJson<QuizDetails[]>(www.text);
            Debug.Log(details.Length);
        }

        else {
            Debug.Log(www.error.ToString());
        }
    }


}