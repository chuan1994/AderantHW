using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public Dictionary<int, GameObject> LandingPositions = new Dictionary<int, GameObject>();

    ////For testing purposes!!!!! REMOVE AFTER
    //[SerializeField]
    //List<GameObject> landPostest = new List<GameObject>();

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
        LandingController.registerPosition += registerLandings;
    }

    void registerLandings(int pos, GameObject go)
    {
        LandingPositions.Add(pos, go);
        //landPostest.Add(go);
    }
}
