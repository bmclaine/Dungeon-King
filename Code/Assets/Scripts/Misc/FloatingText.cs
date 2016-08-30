using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float timer;
    public float speed;

    void Start()
    {
        Destroy(this.gameObject, timer);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
