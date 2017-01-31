using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    string playerID { get; set; }

    [SerializeField]
    string playerName { get; set; }

    [SerializeField]
    bool active { get; set; }

    [SerializeField]
    bool canMove { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
