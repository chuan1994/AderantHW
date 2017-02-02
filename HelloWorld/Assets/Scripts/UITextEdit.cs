using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextEdit : MonoBehaviour {

    Text text;

    private void Awake()
    {
        AssignDelegates();
    }

	// Use this for initialization
	void Start () {
        this.text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AssignDelegates() {
        GameController.setCurrentPlayer += setPlayerName;
    }

    void setPlayerName(GameObject go) {

        string name = go.GetComponent<PlayerController>().playerName;
        if (name.Equals("")) {
            name = "Player " + go.GetComponent<PlayerController>().playerID;
        }

        Debug.Log(name);
        text.text = name + "'s turn!";
    }
}
