using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Vector2 lastCheckPointPos;

    public int areaCode;
    public CinemachineVirtualCamera[] cams;

    // USER INTERFACE

    public bool GamePaused = false;
    public GameObject ExitMenu;
    public GameObject firstButton;

    public bool mainScene = false; // TELLS THE GM IF WE ARE IN THE MAIN SCENE.  IF IT IS FALSE THEN WE WON'T BE ABLE TO ACCES THE PAUSE MENU

    public AudioMixer audioMixer;



    private void Awake()
    {
        // MAKING THIS GAME OBJECT A SINGLETON SO THAT WE CANNOT INSTANTIATE MORE THAN ONE, THIS WOULD CAUSE PROBLEMS
        int numGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numGameManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cams[areaCode].MoveToTopOfPrioritySubqueue();

        PauseToggle();

        PauseMenu();


    }


    public void RestartLVL()
    {
        // RELOAD THE CURRENT SCENE
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);


    }

    public void PauseToggle()
    {
        if (Input.GetButtonDown("Submit") && mainScene == true)
        {
            GamePaused = !GamePaused; // toggle the gamepause boolean

            // CLEAR CURRENT SELECTED GAME OBJECT IN THE EVENT SYSTEM
            EventSystem.current.SetSelectedGameObject(null);
            // SET A NEW SELECTED GAME OBJECT
            EventSystem.current.SetSelectedGameObject(firstButton);

        }

    }

    public void PauseMenu()
    {
        if (GamePaused)
        {
            PauseGame();

        }
        else if (!GamePaused)
        {
            ContinueGame();

        }

    }

    private void PauseGame()
    {
        ExitMenu.SetActive(true); // ACTIVATE THE EXITMENU GAMEOBJECT IN THE SCENE

        // STOP TIME; STAR PLATINUM! ZA WARUDO!
        Time.timeScale = 0f;

        // DISABLE AUDIO
        AudioListener.pause = true;
        
    }

    private void ContinueGame()
    {
        // TIME HAS BEGUN TO MOVE AGAIN
        Time.timeScale = 1f;

        // ENABLE AUDIO
        AudioListener.pause = false;

        ExitMenu.SetActive(false); // DEACTIVATE THE EXITMENU GAMEOBJECT IN THE SCENE

    }

    public void Resume()
    {       
        // TIME HAS BEGUN TO MOVE AGAIN
        Time.timeScale = 1f;

        // ENABLE AUDIO
        AudioListener.pause = false;

        ExitMenu.SetActive(false); // DEACTIVATE THE EXITMENU GAMEOBJECT IN THE SCENE

        GamePaused = !GamePaused;

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        mainScene = true; // TELL THE GAME MANAGER THAT WE ARE IN THE MAIN SCENE NOW

    }

    public void QuitMyGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }





    // END OF FILE
}
