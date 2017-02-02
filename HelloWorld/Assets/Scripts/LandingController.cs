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
        this.GetComponentInChildren<TextMesh>().text = this.country;
        Transform childPos = this.GetComponentInChildren<Transform>();
        childPos.position = transform.position;
        if (this.gameObject.name.Contains("Landing")){
            childPos.rotation = transform.rotation;
        }
        Quaternion.LookRotation(childPos.position, new Vector3(childPos.position.x, 100f, childPos.position.y));
    }

    void setOwner(GameObject go) {
        //this.owner = go;
        Color col = Color.blue;
        Material newMat = new Material(Shader.Find("Standard"));
        newMat.color = col;

        this.gameObject.GetComponent<MeshRenderer>().material = newMat;
    }
}
