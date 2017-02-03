using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoStateController : MonoBehaviour {

    public List<GameObject> canvasItems;

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
        GameController.setStateEvent += setActive;
    }

    void setActive(string name)
    {

        if (name.Equals("gameover")) {
            this.gameObject.SetActive(false);
        }

        foreach (GameObject g in canvasItems)
        {
            if (g.name.ToLower().Equals(name.ToLower()))
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }

    }
}
