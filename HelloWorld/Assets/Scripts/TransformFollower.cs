using UnityEngine;
using System.Collections;

public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 origin = new Vector3(0,15,0);

    private void Awake() {
        AssignDelegates();
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        transform.LookAt(origin);
        Vector3 temp = target.transform.position - origin;
        float angle = Mathf.Atan2(temp.z, temp.x);

        float newX = (160 * Mathf.Cos(angle));
        float newZ = (160 * Mathf.Sin(angle));

        Vector3 newPoint = new Vector3(newX, transform.position.y, newZ);

        Debug.Log(angle);
        transform.position = Vector3.Lerp(transform.position, newPoint, Time.deltaTime * 1f);
    }

    void AssignDelegates() {
        GameController.setCurrentPlayer += setTarget;
    }

    void setTarget(GameObject go) {
        this.target = go;
    }



}