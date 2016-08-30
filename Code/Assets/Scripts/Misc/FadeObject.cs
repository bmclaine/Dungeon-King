using UnityEngine;
using System.Collections;

public class FadeObject : MonoBehaviour
{
    [SerializeField]
    new private Renderer renderer;

    [SerializeField]
    private float startFadeTimer;

    [SerializeField]
    private float fadeSpeed;

    void Update()
    {
        startFadeTimer -= 1.0f * Time.deltaTime;
        if (startFadeTimer > 0.0f) return;

        Color col = renderer.material.color;
        col.a -= 0.25f * Time.deltaTime;
        renderer.material.color = col;
        if (col.a <= 0.0f)
            Destroy(this.gameObject);
    }
}
