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
    private LinkedList<GameObject> players = new LinkedList<GameObject>();

    [SerializeField]
    private int totalTurns;

    public delegate void difficultyEvent(int diff);
    public static event difficultyEvent setGlobalDifficulty;

    public delegate void playerChange(GameObject go);
    public static event playerChange setCurrentPlayer;

    int dieNumber = 0;
    bool dieResponded = false;

    private void Awake()
    {
        AssignDelegates();
    }

	// Use this for initialization
	void Start () {
        setGlobalDifficulty((int)this.difficulty);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void setDifficulty(Difficulty d) {
        this.difficulty = d;
    }

    void AssignDelegates() {
        DieController.GetRandomRoll += DieResonse;
        LandingController.registerPosition += registerLandings;
        PlayerController.registerPlayer += registerPlayers;
        QuestionHandler.PercentSetsAdded += gameReadyCheck;
    }

    IEnumerator runGame()
    {
        int moves = 0;
        while (moves < totalTurns)
        {
            GameObject player = nextPlayer();
            PlayerController playerController = player.GetComponent<PlayerController>();
            while (!dieResponded) {
                //Wait for dice to roll
            }

            dieResponded = false;
            playerController.pos = calcPos(playerController.pos, dieNumber);

            playerController.setNewPos(LandingPositions[playerController.pos].transform.position);


            yield return new WaitForSeconds(1);
        }
    }


    void registerLandings(int pos, GameObject go){
        LandingPositions.Add(pos, go);
    }

    void gameReadyCheck(int percent) {
        if (percent == 100) {
            StartCoroutine(runGame());
        }
    }

    void registerPlayers(GameObject go) {
        players.AddLast(go);

    }

    void DieResonse(int value) {
        dieNumber = value;
        dieResponded = true;
    }

    GameObject nextPlayer() {
        GameObject nextPlayer = players.First.Value;
        players.RemoveFirst();
        players.AddLast(nextPlayer);
        return nextPlayer;
    }

    int calcPos(int current, int increase) {
        int value = current + increase;
        if (value <= 43) {
            return value;
        }

        return value - 44;
    }
}
