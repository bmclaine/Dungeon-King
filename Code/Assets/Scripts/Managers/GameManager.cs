using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private static GameManager manager;
    public static GameManager instance
    {
        get
        {
            if (manager)
                return manager;

            manager = FindObjectOfType<GameManager>();
            return manager;
        }
    }

    public int soulsNeeded;

    public GameObject dieInterface;
    public GameObject gameOverInterface;
    public RectTransform dieBackground;
    public RectTransform gameOverBackground;
    public Text soulsNeededText;
    public Text playerSoulsText;
    public bool showInterface;

    private Player player;

    [SerializeField]
    private Selectable gameOverButton;

    [SerializeField]
    private Selectable reviveButton;

    public bool isGameOver = false;

    public void PlayerDie(Player _player)
    {
        isGameOver = true;
        player = _player;
        if (!showInterface) return;

        ShowInterface();
    }

    public void ShowInterface()
    {
        if (player.SoulCount >= soulsNeeded)
        {
            dieInterface.SetActive(true);
            Vector2 size = new Vector2(Screen.width, Screen.height);
            dieBackground.sizeDelta = size;

            soulsNeededText.text = soulsNeeded.ToString();
            playerSoulsText.text = player.SoulCount.ToString();

            EventSystem.current.SetSelectedGameObject(reviveButton.gameObject);
        }
        else
        {
            Vector2 size = new Vector2(Screen.width, Screen.height);
            gameOverBackground.sizeDelta = size;
            gameOverInterface.SetActive(true);
            dieInterface.SetActive(false);
            EventSystem.current.SetSelectedGameObject(gameOverButton.gameObject);
        }
    }

    public void Win()
    {
        isGameOver = false;
        Time.timeScale = 1.0f;

        Application.LoadLevel("Win Scene");
    }

    public void RevivePlayer()
    {
        if (player == null)
            return;

        if (!showInterface)
            return;

        player.ResetHealth();
        player.ChangeState(PlayerState.Idle);
        dieInterface.SetActive(false);

        EntityManager.instance.AddPlayer(player);

        isGameOver = false;
    }

    private void Quit()
    {
        PersistentInfo.LoadLevel("Main Menu");
    }

}
