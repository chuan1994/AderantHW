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
    private bool canMove = false;

    [SerializeField]
    private Color color;

    [SerializeField]
    private int initPos;

    public delegate void register(GameObject go);
    public static event register registerPlayer;

    private Vector3 origin;
    private Vector3 target;

    // Use this for initialization
    void Start () {
        origin = new Vector3(0, transform.position.y, 0);
        target = this.transform.position;
        registerPlayer(gameObject);

        StartCoroutine(test());
    }
	
	// Update is called once per frame
	void Update () {
        Quaternion targetRotation = Quaternion.LookRotation(origin - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 1f);

        if (Vector3.Distance(transform.position, target) > 0.1f && canMove)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 1f);
        }
        else {
            canMove = false;
            transform.position = target;
            //ALERT DONE!!!.
        }
	}

    void setNewPos(Vector3 newPos) {
        target = newPos;
    }

    IEnumerator test() {
        while (true) {
            yield return new WaitForSeconds(10);
            target = new Vector3(target.x, target.y, target.z * -1f);
            canMove = true;
        }
    }
}
