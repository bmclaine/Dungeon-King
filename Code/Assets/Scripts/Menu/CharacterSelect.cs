using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CharacterSelect : MonoBehaviour 
{
    [System.Serializable]
    private struct PlayerDataDisplay
    {
        public GameObject parentObject;
        public Text playerName;
        public Image healthBar;
        public Image manaBar;
        public Image pAttackBar;
        public Image pDefenseBar;
        public Image eAttackBar;
        public Image eDefenseBar;
    }

    private int characterChoice = 0;

    [SerializeField]
    private Transform cameraObject;
    [SerializeField]
    private Transform[] targetObject;
    [SerializeField]
    private Transform[] lightTransform;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float stoppingDistance;

    [SerializeField]
    private PlayerDataDisplay playerDisplay;

    [SerializeField]
    private PlayerInfo maxStats;

    [SerializeField]
    private PlayerInfo[] finalStats;

    [SerializeField]
    private Selectable firstSelected;

    [SerializeField]
    private AudioClip changeChoiceSound;

    private bool moving = false;
    private float delay = 0.2f;

    private void Start()
    {
        SetDisplayInfo();

        firstSelected.Select();
    }

    private void Update()
    {
        input();

        if (moving)
            moveCamera();
    }

    private void input()
    {   
        if (Input.GetAxis("Horizontal") > 0.0f && delay <= 0.0f && !moving)
        {
            delay = 0.2f;
            lightTransform[characterChoice].gameObject.SetActive(false);
            UpdateChoice(1);
            playerDisplay.parentObject.SetActive(false);
            moving = true;

            PlaySound();
        }
        else if (Input.GetAxis("Horizontal") < 0.0f && delay <= 0.0f && !moving)
        {
            delay = 0.2f;
            lightTransform[characterChoice].gameObject.SetActive(false);
            UpdateChoice(-1);
            playerDisplay.parentObject.SetActive(false);
            moving = true;

            PlaySound();
        }

        if(Input.GetButtonDown("Cancel"))
        {
            MainMenu();
        }

        delay -= Time.deltaTime;

        if (EventSystem.current.currentSelectedGameObject == null)
            firstSelected.Select();
    }

    private void UpdateChoice()
    {
        characterChoice++;

        if (characterChoice > 2)
            characterChoice = 0;
    }

    private void UpdateChoice(int value)
    {
        switch(characterChoice)
        {
            case 0:
                if (value == 1)
                    characterChoice = 1;
                else
                    characterChoice = 2;
                break;
            case 1:
                if (value == 1)
                    characterChoice = 0;
                else
                    characterChoice = 2;
                break;
            case 2:
                if (value == 1)
                    characterChoice = 1;
                else
                    characterChoice = 0;
                break;
        }
        //characterChoice += value;

        //if (characterChoice > 2)
        //    characterChoice = 0;

        //if (characterChoice < 0)
        //    characterChoice = 2;
    }

    private void moveCamera()
    {
        float distance = Vector3.Distance(targetObject[characterChoice].position, cameraObject.position);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 targetPos = targetObject[characterChoice].position - transform.position;

        targetPos.Normalize();

        float dot = Vector3.Dot(forward, targetPos);

        if (distance > stoppingDistance && dot < 0.995f)
        {
            cameraObject.position = Vector3.MoveTowards(cameraObject.position, targetObject[characterChoice].position, moveSpeed * Time.deltaTime);
            //targetObject.RotateAround(targetObject.position, Vector3.up * direction, rotationSpeed * Time.deltaTime);
        }
        else
        {
            lightTransform[characterChoice].gameObject.SetActive(true);
           
            moving = false;
            SetDisplayInfo();
        }
    }

    private void SetDisplayInfo()
    {
        playerDisplay.parentObject.SetActive(true);

        float finalHealth = finalStats[characterChoice].health.max / maxStats.health.max;
        float finalMana = finalStats[characterChoice].mana.max / maxStats.mana.max;
        float finalpAtk = finalStats[characterChoice].attack.physical / maxStats.attack.physical;
        float finalpDef = finalStats[characterChoice].defense.physical / maxStats.defense.physical;
        float finaleAtk = finalStats[characterChoice].attack.elemental / maxStats.attack.elemental;
        float finaleDef = finalStats[characterChoice].defense.elemental / maxStats.defense.elemental;

        playerDisplay.playerName.text = ((PlayerType)characterChoice).ToString();
        playerDisplay.healthBar.fillAmount = finalHealth;
        playerDisplay.manaBar.fillAmount = finalMana;
        playerDisplay.pAttackBar.fillAmount = finalpAtk;
        playerDisplay.pDefenseBar.fillAmount = finalpDef;
        playerDisplay.eAttackBar.fillAmount = finaleAtk;
        playerDisplay.eDefenseBar.fillAmount = finaleDef;
    }

    private void PlaySound()
    {
        if (SoundManager.instance)
            SoundManager.instance.PlayClip(changeChoiceSound, Vector3.zero);
    }

    public void SelectCharacter()
    {
        PersistentInfo.selectedPlayer = characterChoice;
        PersistentInfo.LoadLevel("NewGameMenu");
    }

    public void MainMenu()
    {
        PersistentInfo.LoadLevel("Main Menu");
    }
}
