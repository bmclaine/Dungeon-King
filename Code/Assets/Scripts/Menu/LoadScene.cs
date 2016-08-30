using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadScene : MonoBehaviour 
{
    [System.Serializable]
    private struct LoadDataDisplay
    {
        public GameObject parentObject;
        public Text playerType;
        public Text playerLevel;
        public Text playerElement;
    }

    [SerializeField]
    private Transform cameraObject;
    [SerializeField]
    private GameObject[] playerObject;
    [SerializeField]
    private string[] loadSlotsNames;
    [SerializeField]
    private LoadDataDisplay loadDataDisplay;

    private SaveData[] loadData;

    [SerializeField]
    private Transform[] anchorPoints;
    [SerializeField]
    private Transform[] destinations;

    [SerializeField]
    private float stoppingDistance;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Selectable selectedOption;
    [SerializeField]
    private AudioClip changeChoiceSound;
    private int choice = 0;
    private bool moving = false;
    private float delay = 0.2f;

    private List<GameObject> playerObjects = new List<GameObject>();

	private void Start () 
    {
        if (selectedOption)
            selectedOption.Select();

        Load();

        SetLoadInfo();

        CreatePlayerObjects();
	}
	
	
	private void Update () 
    {
        input();

        if (moving)
            moveCamera();
	}

    private void input()
    {
        if (choice > 1)
        {
            if (Input.GetAxis("Horizontal") < 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(-1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }

            if (Input.GetAxis("Horizontal") > 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }
        }
        else if (choice < 1)
        {
            if (Input.GetAxis("Horizontal") < 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }

            if (Input.GetAxis("Horizontal") > 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(-1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(-1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }

            if (Input.GetAxis("Horizontal") > 0.0f && delay <= 0.0f && moving == false)
            {
                delay = 0.2f;
                UpdateChoice(1);
                loadDataDisplay.parentObject.SetActive(false);
                moving = true;

                if (SoundManager.instance)
                    SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
            }
        }

        delay -= Time.deltaTime;
    }

    private void moveCamera()
    {
        float distance = Vector3.Distance(destinations[choice].position, cameraObject.position);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 targetPos = destinations[choice].position - transform.position;

        targetPos.Normalize();

        float dot = Vector3.Dot(forward, targetPos);

        if (distance > stoppingDistance && dot < 0.995f)
        {
            cameraObject.position = Vector3.MoveTowards(cameraObject.position, destinations[choice].position, moveSpeed * Time.deltaTime);
        }
        else
        {
            loadDataDisplay.parentObject.SetActive(true);
            selectedOption.Select();
            moving = false;
            SetLoadInfo();
        }
    }

    private void UpdateChoice()
    {
        ++choice;

        if (choice > 2)
            choice = 0;
    }

    private void UpdateChoice(int value)
    {
        choice += value;

        if (choice < 0)
            choice = 2;

        if (choice > 2)
            choice = 0;
    }

    private void Load()
    {
        loadData = new SaveData[3];

        for (int i = 0; i < loadData.Length; ++i)
        {
            loadData[i] = SaveData.LoadPrefs(loadSlotsNames[i]);
            if (loadData[i].isLoadSuccessful == false)
            {
                loadData[i].level = PersistentInfo.instance.defaultLevelName;
                loadData[i].playerType = PlayerType.None;
                loadData[i].playerLevel = 0;
            }
        }
    }

    private void SetLoadInfo()
    {
        loadDataDisplay.playerType.text = loadData[choice].playerType.ToString();
        loadDataDisplay.playerLevel.text = "Lvl. " + loadData[choice].playerLevel;
        loadDataDisplay.playerElement.text = "Element: " + loadData[choice].playerElement;
    }

    private void CreatePlayerObjects()
    {
        for(int i = 0; i < loadData.Length; ++i)
        {
            int playerType = (int)loadData[i].playerType;
            GameObject obj = (GameObject)Instantiate(playerObject[playerType], anchorPoints[i].position, anchorPoints[i].rotation);
            playerObjects.Add(obj);
        }
    }

    public void LoadData()
    {
        PersistentInfo.slotName = loadSlotsNames[choice];
        PersistentInfo.selectedPlayer = (int)loadData[choice].playerType;
        PersistentInfo.saveData = loadData[choice];
        PersistentInfo.LoadLevel(loadData[choice].level);
    }

    public void GoToMainMenu()
    {
        PersistentInfo.LoadLevel("Main Menu");
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(loadSlotsNames[choice]);
        loadData[choice] = SaveData.Load(loadSlotsNames[choice]);
        loadData[choice].playerType = PlayerType.None;
        Destroy(playerObjects[choice]);
        int playerType = (int)loadData[choice].playerType;
        GameObject obj = (GameObject)Instantiate(playerObject[playerType], anchorPoints[choice].position, anchorPoints[choice].rotation);
        playerObjects[choice] = obj;
        SetLoadInfo();
    }
}
