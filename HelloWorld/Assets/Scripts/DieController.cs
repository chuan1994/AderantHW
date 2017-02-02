using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour {

    [SerializeField]
    int maxNumOnDie = 6;

    public delegate void rollDice(int value);
    public static event rollDice GetRandomRoll;

    private void Awake()
    {
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DiceRoll() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int value = Random.Range(0, maxNumOnDie) + 1;
        new WaitForSeconds(3);
        GetRandomRoll(value);
    }
}
