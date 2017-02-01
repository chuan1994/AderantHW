using UnityEngine;
using System.Collections;

public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private Vector3 origin = new Vector3(0,0,0);

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
        Vector3 temp = target.position - origin;
        float angle = Mathf.Atan2(temp.z, temp.x);

        float newX = (160 * Mathf.Cos(angle));
        float newZ = (160 * Mathf.Sin(angle));

        Vector3 newPoint = new Vector3(newX, transform.position.y, newZ);

        Debug.Log(angle);
        transform.position = Vector3.Lerp(transform.position, newPoint, Time.deltaTime * 0.5f);

    }
}