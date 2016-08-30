using UnityEngine;
using System.Collections;

public class MessageWindow : MonoBehaviour
{
    [SerializeField]
    private string message;
    private float timer;
    private bool messageFade;
    private bool messageRead;

    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
        messageFade = false;
        messageRead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                messageFade = true;
            }
        }

        if (messageFade)
        {
            float alpha = HUDInterface.instance.FadeMessage();

            if (alpha <= 0.0f)
            {
                messageFade = false;
                HUDInterface.instance.ClearMessageWindow();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (messageRead) return;

        Player player = col.GetComponent<Player>();
        if (player == null) return;

        HUDInterface.instance.SetMessageWindow(message);
        timer = 5.0f;
        messageRead = true;
    }
}
