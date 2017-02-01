using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    private Dictionary<int, GameObject> LandingPositions = new Dictionary<int, GameObject>();

    public enum Difficulty : int { Easy=400, Medium=600, Hard=1200, VeryHard=int.MaxValue }

    [SerializeField]
    private Difficulty difficulty;

    [SerializeField]
    private List<GameObject> players = new List<GameObject>(); 

    public delegate void difficultyEvent(int diff);
    public static event difficultyEvent setGlobalDifficulty;

    public delegate void playerChange(GameObject go);
    public static event playerChange setCurrentPlayer;

    private void Awake()
    {
        AssignDelegates();
    }


	// Use this for initialization
	void Start () {
        setGlobalDifficulty((int)this.difficulty);
        StartCoroutine(test());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setDifficulty(Difficulty d) {
        this.difficulty = d;
    }

    void AssignDelegates() {
        LandingController.registerPosition += registerLandings;
        PlayerController.registerPlayer += registerPlayers;
    }

    void registerLandings(int pos, GameObject go){
        LandingPositions.Add(pos, go);
    }

    void registerPlayers(GameObject go) {
        players.Add(go);
    }

    IEnumerator test() {
        while (true) {
            GameObject go = randomPlayer();
            Debug.Log(go.name);
            setCurrentPlayer(go);
            yield return new WaitForSeconds(3);
        }
    }

    GameObject randomPlayer() {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        return players[UnityEngine.Random.Range(0, players.Count)];
    }
}
