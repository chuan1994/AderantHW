using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int playerID;
    [SerializeField]
    private string playerName;

    [SerializeField]
    private bool active;

    [SerializeField]
    private bool canMove;

    [SerializeField]
    private Color color;

    public delegate void register(GameObject go);
    public static event register registerPlayer;

    private Vector3 origin;

    // Use this for initialization
    void Start () {
        origin = new Vector3(0, transform.position.y, 0);
        registerPlayer(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion targetRotation = Quaternion.LookRotation(origin - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
	}
}
