using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour {

    [SerializeField]
    int pos;

    public delegate void register(int pos, GameObject go);
    public static event register registerPosition;

	// Use this for initialization
	void Start () {
        registerPosition(pos, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
