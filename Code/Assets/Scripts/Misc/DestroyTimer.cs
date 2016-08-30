using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour 
{
    public float timer;

    void Start()
    {
        Destroy(this.gameObject, timer);
    }
}
