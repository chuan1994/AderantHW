using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

public class GameController : MonoBehaviour {


    private Dictionary<int, GameObject> LandingPositions = new Dictionary<int, GameObject>();

    public enum Difficulty : int { Easy=400, Medium=600, Hard=1200, VeryHard=int.MaxValue }

    [SerializeField]
    private Difficulty difficulty;

    [SerializeField]
    private LinkedList<GameObject> players = new LinkedList<GameObject>();

    [SerializeField]
    private int totalTurns;

    public delegate void emptyDelegate();
    public static event emptyDelegate cameraOverHead;

    public delegate void intDelegate(int diff);
    public static event intDelegate setGlobalDifficulty;

    public delegate void gameObjectDelegate(GameObject go);
    public static event gameObjectDelegate setCurrentPlayer;

    public delegate void stringDelegate(string state);
    public static event stringDelegate setStateEvent;
    public static event stringDelegate setUIText;

    public delegate Question questionReturnDelegate(string country);
    public static event questionReturnDelegate getQuestion;

    public delegate void questionStringDelegate(Question question, String country);
    public static event questionStringDelegate setQuestion;

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

                if (landingSpot.GetComponent<LandingController>().owner != null) {
                    continue;
                }

                //WAIT FOR QUESTION
                setStateEvent("QuestionState");
                yield return StartCoroutine(QuestionWait(player, landingSpot));
            }
          
            yield return new WaitForSeconds(2);

            moves++;
        }

        setUIText("Game Over!");
        setStateEvent("gameover");
        yield return new WaitForSeconds(1);
        DisplayEndScene();
    }


    IEnumerator QuestionWait(GameObject player, GameObject landingSpot) {
        Question q = getQuestion(landingSpot.GetComponent<LandingController>().country);
        setQuestion(q, landingSpot.GetComponent<LandingController>().country);

        while (!responded)
        {
            yield return new WaitForEndOfFrame();
        }
        responded = false;

        if (correctAnswer(this.answer, q.answer))
        {
            setUIText("Congratulations! You are correct!");
            if (landingSpot.GetComponent<LandingController>().owner == null) {
                landingSpot.GetComponent<LandingController>().setOwner(player);
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

    void DisplayEndScene()
    {
        string result = CalcWinner();
        //setUIText(result);
        cameraOverHead();
    }

    string CalcWinner() {

        Dictionary<GameObject, int> winMap = new Dictionary<GameObject, int>();
        foreach (GameObject go in players) {
            winMap.Add(go, 0);
        }

        foreach (GameObject go in LandingPositions.Values) {
            if (go.GetComponent<LandingController>().owner != null) {
                winMap[go.GetComponent<LandingController>().owner] = winMap[go.GetComponent<LandingController>().owner] + 1;
            }
        }
        StringBuilder sb = new StringBuilder();

        int max = winMap.Values.Max();
        List<GameObject> winners = new List<GameObject>();

        foreach (GameObject go in winMap.Keys) {
            if (winMap[go] == max) {
                winners.Add(go);
            }
        }

        if (winners.Count == 1)
        {
            sb.Append("The winner is " + winners[0].GetComponent<PlayerController>().playerName);
        }
        else {
            sb.Append("The winner's are:");
            foreach (GameObject go in winners) {
                sb.Append("\n");
                sb.Append(go.GetComponent<PlayerController>().playerName);
            }

            sb.Append("!");
        }

        sb.Append("\nScore : " + max);

        return sb.ToString();
    }

}
