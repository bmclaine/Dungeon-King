using UnityEngine;
using System.Collections;

public class AnimationWait : MonoBehaviour
{
    [SerializeField]
    private float timer;

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
            animator.enabled = true;
    }
}
