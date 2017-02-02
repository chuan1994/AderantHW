using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    public int playerID;
    [SerializeField]
    public string playerName;

    [SerializeField]
    private bool active;

    [SerializeField]
    private bool canMove = false;

    [SerializeField]
    private bool moving = false;

    [SerializeField]
    private Color color;

    [SerializeField]
    private int initPos;

    public delegate void register(GameObject go);
    public static event register registerPlayer;

    private Vector3 origin;
    private Vector3 target;

    private void Awake()
    {
        AssignDelegates();
    }

    // Use this for initialization
    void Start () {
        origin = new Vector3(0, transform.position.y, 0);
        target = this.transform.position;
        registerPlayer(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        Quaternion targetRotation = Quaternion.LookRotation(origin - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 1f);

        if ((Vector3.Distance(transform.position, target) > 0.1f) && (canMove || moving))
        {
            moving = true;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 1f);
        }
        else {
            moving = false;
            canMove = false;
            transform.position = target;
            //ALERT DONE!!!.
        }
	}

    void setNewPos(Vector3 newPos) {
        target = newPos;
    }

    void SetActive(GameObject go) {
        if (go.Equals(this.gameObject))
        {
            gameObject.layer = LayerMask.NameToLayer("Outline");
            this.canMove = true;
            this.active = true;
            target = new Vector3(target.x, target.y, target.z * -1f);
        }
        else {
            gameObject.layer = LayerMask.NameToLayer("Default");
            this.canMove = false;
            this.active = false;
        }
    }

    void moveTo(Vector3 location) {
        if (this.active) {
            target = location;
        }
    }

    void AssignDelegates() {
        GameController.setCurrentPlayer += SetActive;
    }
}
