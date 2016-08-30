using UnityEngine;
using System.Collections;

public class RotateAroundTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float angle;
    [SerializeField]
    private Space space;

    private void Update()
    {
        if (!target)
            return;

        transform.RotateAround(target.position, Vector3.up, angle * Time.deltaTime);
    }
}
