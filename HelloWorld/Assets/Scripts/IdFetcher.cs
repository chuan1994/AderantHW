using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
            string result = ProcessResponse(www.text);
            Wrapper<quizDetails> questionList;
            questionList = JsonUtility.FromJson<Wrapper<quizDetails>>(result);

            Debug.Log(questionList.items.Length);

        }

        else {
            Debug.Log(www.error.ToString());
        }
    }

    string ProcessResponse(string input) {
        string pattern = "(\\?( )*\\()(.+)(\\)( )*)";
        Match match = Regex.Match(input, pattern);
        string result = match.Groups[3].ToString();

        StringBuilder sb = new StringBuilder();
        sb.Append("{\"items\" : ");
        sb.Append(result);
        sb.Append("}");

        string output = sb.ToString();

        Debug.Log(output.Substring(0, 200));
        Debug.Log(output.Substring(output.Length - 300));
        return sb.ToString();
    }


}