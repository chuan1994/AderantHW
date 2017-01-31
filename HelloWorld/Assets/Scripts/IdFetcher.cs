using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdFetcher : MonoBehaviour {

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
            Debug.Log(www.text);
        }
        else {
            Debug.Log(www.error.ToString());
        }
    }


}