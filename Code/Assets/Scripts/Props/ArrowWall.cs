using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArrowWall : MonoBehaviour
{
    // set in inspector
    [SerializeField]
    private List<Transform> projectilePoints;
    // filled with 15 random projectilePoints
    [SerializeField]
    private int[] selectedIndexes;

    [SerializeField]
    private GameObject arrowTemplate;

    [SerializeField]
    private AudioClip plate;
    [SerializeField]
    private AudioClip arrowFire;

    private Animator animator;

    private int maxIndex;
    private bool active;
    private float timer;
    private int index;

    void Start()
    {
        selectedIndexes = new int[15];
        active = false;
        timer = 0.0f;
        RandomizePoints();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                active = false;
                animator.SetBool("isActive", active);

                SoundManager.instance.PlayClip(plate, transform.position);
            }
        }
    }

    void RandomizePoints()
    {
        for (int i = 0; i < selectedIndexes.Length; ++i)
        {
            int randomNum = Random.Range(0, selectedIndexes.Length - 1);
            selectedIndexes[i] = randomNum;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (active) return;

        Player player = col.GetComponent<Player>();
        if (player == null) return;

        active = true;
        animator.SetBool("isActive", active);

        if(SoundManager.instance)
            SoundManager.instance.PlayClip(plate, transform.position);

        timer = 5.0f;

        index = 0;

        InvokeRepeating("FireProjectile", 0.0f, 0.05f);
    }

    private void FireProjectile()
    {
        Transform point = projectilePoints[selectedIndexes[index]];

        if (!point)
        {
            print("No Point");
            CancelInvoke("FireProjectile");
            return;
        }

        Vector3 position = point.position;
        Quaternion rotation = point.rotation;

        Instantiate(arrowTemplate, position, rotation);

        if (SoundManager.instance)
            SoundManager.instance.PlayClip(arrowFire, transform.position);

        index++;

        if (index == 15)
        {
            CancelInvoke("FireProjectile");
        }
    }
}
