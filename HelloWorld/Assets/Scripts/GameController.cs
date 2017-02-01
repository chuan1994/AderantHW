using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public Dictionary<int, GameObject> LandingPositions = new Dictionary<int, GameObject>();

    public enum Difficulty : int { Easy=400, Medium=600, Hard=1200, VeryHard=int.MaxValue }

    public Difficulty difficulty;

    public delegate void difficultyEvent(int diff);
    public static event difficultyEvent setGlobalDifficulty;

    ////For testing purposes!!!!! REMOVE AFTER
    //[SerializeField]
    //List<GameObject> landPostest = new List<GameObject>();

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
        LandingController.registerPosition += registerLandings;
    }

    void registerLandings(int pos, GameObject go)
    {
        LandingPositions.Add(pos, go);
        //landPostest.Add(go);
    }
}
