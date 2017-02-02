using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {


    private Dictionary<int, GameObject> LandingPositions = new Dictionary<int, GameObject>();

    public enum Difficulty : int { Easy=400, Medium=600, Hard=1200, VeryHard=int.MaxValue }

    [SerializeField]
    private Difficulty difficulty;

    [SerializeField]
    private LinkedList<GameObject> players = new LinkedList<GameObject>();

    [SerializeField]
    private int totalTurns;

    public delegate void intDelegate(int diff);
    public static event intDelegate setGlobalDifficulty;

    public delegate void gameObjectDelegate(GameObject go);
    public static event gameObjectDelegate setCurrentPlayer;

    public delegate void stringDelegate(string state);
    public static event stringDelegate setStateEvent;
    public static event stringDelegate setUIText;

    public delegate Question questionReturnDelegate(string country);
    public static event questionReturnDelegate getQuestion;

    public delegate void questionDelegate(Question question);
    public static event questionDelegate setQuestion;

    int dieNumber = 0;
    string answer = "";
    bool responded = false;
    bool correct = false;

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
        QuestionDisplay.sendAnswer += answerSubmit;
    }

    IEnumerator runGame()
    {
        int moves = 0;
        while (moves < totalTurns)
        {
            yield return new WaitForFixedUpdate();
            GameObject player = nextPlayer();

            setCurrentPlayer(player);

            //CHECK IF CURRENT POSITION IS OWNED BY ANOTHER PLAYER
            correct = true;
            yield return new WaitForSeconds(1);

            GameObject currentLanding = LandingPositions[player.GetComponent<PlayerController>().pos];
            if (currentLanding.GetComponent<LandingController>().owner != null  && currentLanding.GetComponent<LandingController>().owner != player) {
                setUIText("This is owned by " + currentLanding.GetComponent<LandingController>().owner.GetComponent<PlayerController>().playerName);
                yield return new WaitForSeconds(1);
                setUIText("Get this question right to continue");

                setStateEvent("QuestionState");
                yield return StartCoroutine(QuestionWait(player, currentLanding));
            }


            if (correct) {
                //DICE ROLL THEN WAIT FOR MOVEMENT
                setStateEvent("DiceState");

                PlayerController playerController = player.GetComponent<PlayerController>();
                while (!responded)
                {
                    yield return new WaitForEndOfFrame();
                }
                responded = false;

                playerController.pos = calcPos(playerController.pos, dieNumber);
                GameObject landingSpot = LandingPositions[playerController.pos];
                Vector3 newPos = landingSpot.transform.position;
                newPos.y = player.transform.position.y;
                playerController.moveTo(newPos);
                yield return new WaitForSeconds(3);

                //WAIT FOR QUESTION
                setStateEvent("QuestionState");
                yield return StartCoroutine(QuestionWait(player, landingSpot));
            }
          
            yield return new WaitForSeconds(4);

            moves++;
        }

        yield return null;
    }


    IEnumerator QuestionWait(GameObject player, GameObject landingSpot) {
        Question q = getQuestion(landingSpot.GetComponent<LandingController>().country);
        setQuestion(q);

        while (!responded)
        {
            yield return new WaitForEndOfFrame();
        }
        responded = false;

        if (correctAnswer(this.answer, q.answer))
        {
            setUIText("Congratulations! You are correct!");
            if (landingSpot.GetComponent<LandingController>().owner == null) {
                landingSpot.GetComponent<LandingController>().owner = player;
                //WIN SEQUENCEEEEEE!!!!!!!!!!!!!!!
            }
        }
        else
        {
            setUIText("Sorry, the correct answer was " + q.answer);
            //LOSE SEQUENCE!!!!!!!!!!!!!
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

    void answerSubmit(String answer) {
        this.answer = answer;
        responded = true;
    }

    void DieResonse(int value) {
        dieNumber = value;
        responded = true;
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

    bool correctAnswer(string a, string b) {
        Regex reg = new Regex("(?i)(the|a|an)");
        a = reg.Replace(a.Trim().ToLower(), "");
        b = reg.Replace(b.Trim().ToLower(), "");

        if (a.Trim().ToLower().Equals(b.Trim().ToLower())){
            correct = true;
            return true;
        }

        correct = false;
        return false;
    }
}
