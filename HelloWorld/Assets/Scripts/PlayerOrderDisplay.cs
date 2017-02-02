using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOrderDisplay : MonoBehaviour {

    [SerializeField]
    private List<InputField> textFields;

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        AssignDelegates();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AssignDelegates() {
        PlayerController.registerPlayer += registerPlayer;
        QuestionHandler.PercentSetsAdded += onGameReady;
    }

    void registerPlayer(GameObject go) {
        Debug.Log(go.name);
        InputField input = textFields[players.Count];
        players.Add(go);

        ColorBlock cb = ColorBlock.defaultColorBlock;
        Color color = go.GetComponent<PlayerController>().color;
        color.a = 1;
        cb.normalColor = color;
        color.a = 0.5f;
        cb.highlightedColor = color; 
        input.colors = cb;
        input.text = "Player " + go.GetComponent<PlayerController>().playerID;
    }

    void onGameReady(int percent) {
        if (percent != 100) {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            players[i].GetComponent<PlayerController>().playerName = textFields[i].text;
        }

    }
}
