using UnityEngine;
using System.Collections;

public class MimicChest : MonoBehaviour
{
    private bool isOpen;

    [SerializeField]
    private AudioClip chestOpen;
    [SerializeField]
    private AudioClip chestClose;

    [SerializeField]
    private EnemySpawner spawner;

    private Animator animator;
    new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        isOpen = false;
        animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            Color color = renderer.material.color;
            color.a -= Time.deltaTime * 0.3f;
            renderer.material.color = color;

            if (color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (player == null) return;

        if (Input.GetButtonDown("Interact") && !isOpen)
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);

            if (SoundManager.instance)
                SoundManager.instance.PlayClip(chestOpen, Vector3.zero);

            spawner.Activate();
        }
    }
}
