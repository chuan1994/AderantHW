using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryAssignment : MonoBehaviour {

    [SerializeField]
    List<string> countries = new List<string>();

    [SerializeField]
    List<string> assigningDuplicate = new List<string>();

    public delegate void ListReady(List<string> countries);
    public static event ListReady ListReadyEvent;

    void Awake()
    {
        assigningDuplicate.AddRange(countries);
        AssignDelegate();
    }

	// Use this for initialization
	void Start () {

        ListReadyEvent(countries);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void AssignDelegate() {
        LandingController.registerPosition += AssignCountry;
    }

    void AssignCountry(int pos, GameObject go) {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int rand = Random.Range(0, assigningDuplicate.Count);
        go.SendMessage("setCountry", assigningDuplicate[rand]);
        assigningDuplicate.RemoveAt(rand);
    }
}
