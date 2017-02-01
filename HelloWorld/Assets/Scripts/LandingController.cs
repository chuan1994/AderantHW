using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingController : MonoBehaviour {

    [SerializeField]
    public int pos;

    [SerializeField]
    public string country;

    [SerializeField]
    public GameObject owner;

    public delegate Question activated(string country);
    public static event activated landingActivated;

    //delegate-event to register gameobject as a landing object
    public delegate void register(int pos, GameObject go);
    public static event register registerPosition;

	// Use this for initialization
	void Start () {
        registerPosition(pos, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void setCountry(string country)
    {
        this.country = country;
    }

    void OnMouseDown() {
        //QuestionHandler.printQuestion(landingActivated(this.country));
        setOwner(this.gameObject);
    }

    void setOwner(GameObject go) {

        //this.owner = go;
        Color col = Color.blue;
        Material newMat = new Material(Shader.Find("Standard"));
        newMat.color = col;

        this.gameObject.GetComponent<MeshRenderer>().material = newMat;
    }
}
