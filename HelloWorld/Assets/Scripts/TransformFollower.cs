using UnityEngine;
using System.Collections;

public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 origin = new Vector3(0,15,0);

    [SerializeField]
    private bool followTarget = false;

    [SerializeField]
    private Transform overviewPos;

    private void Awake() {
        AssignDelegates();
    }

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (target == null)
        {
            return;
        }


        if (followTarget)
        {

            Vector3 temp = target.transform.position - origin;
            float angle = Mathf.Atan2(temp.z, temp.x);

            float newX = (160 * Mathf.Cos(angle));
            float newZ = (160 * Mathf.Sin(angle));

            Vector3 newPoint = new Vector3(newX, 40f , newZ);

            transform.position = Vector3.Lerp(transform.position, newPoint, Time.deltaTime * 5f);
            transform.LookAt(origin);
        }
        else {
            transform.position = Vector3.Lerp(transform.position, overviewPos.position, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, overviewPos.rotation, Time.deltaTime * 5f);
        }
    }

    void AssignDelegates() {
        GameController.setCurrentPlayer += setTarget;
        GameController.cameraOverHead += winScreen;
    }

    void setTarget(GameObject go) {
        this.target = go;
        followTarget = true;
    }

    void winScreen()
    {
        followTarget = false;
    }
}