using UnityEngine;
using System;
using System.Collections.Generic;

public enum GameStates
{
    PLAYING,
    PAUSED,
    GAME_OVER,
    NUM_STATES
}

public class GameManager : MonoBehaviour
{
    public GameStates curState = GameStates.PLAYING;
    public static GameManager instance;
    private Dictionary<GameStates, Action> fsm = new Dictionary<GameStates, Action>();

    //Start events per state
    public event Action EventStartPlaying;
    public event Action EventStartPaused;
    public event Action EventStartGameOver;

    //Update events per state
    public event Action EventPlaying;
    public event Action EventPaused;
    public event Action EventGameOver;

    //End events per state
    public event Action EventEndPlaying;
    public event Action EventEndPaused;
    public event Action EventEndGameOver;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        if(instance != null && instance != this){
            //Debug.Log("Destroying this instance.");
            Destroy(this);
        }
        else instance = this;
        //Debug.Log("Setting this instance.");
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        fsm.Add(GameStates.PLAYING, StatePlaying);
        fsm.Add(GameStates.PAUSED, StatePaused);
        fsm.Add(GameStates.GAME_OVER, StateGameOver);
        SetState(GameStates.PLAYING);
    }
    // Update is called once per frame
    void Update()
    {
        fsm[curState].Invoke();
    }

    public void SetState(GameStates newState){
        if(newState != curState){
            EndState();
            curState = newState;
            BeginState();
        }
    }

    //Gets called once on change from this state
    void EndState()
    {
        switch (curState)
        {
            case GameStates.PLAYING:
                EndPlaying();
                break;
            case GameStates.PAUSED:
                EndPaused();
                break;
            case GameStates.GAME_OVER:
                EndGameOver();
                break;
            default:
                break;
        }
    }
    
    //Gets called once on change to this state
    void BeginState()
    {
        switch (curState)
        {
            case GameStates.PLAYING:
                StartPlaying();
                break;
            case GameStates.PAUSED:
                StartPaused();
                break;
            case GameStates.GAME_OVER:
                StartGameOver();
                break;
            default:
                break;
        }
    }

    //Public access
    //Call to pause the game
        public void PauseGame()
    {
        Time.timeScale = 0f;
        SetState(GameStates.PAUSED);
    }
    //Call to resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetState(GameStates.PLAYING);
    }

    //Start event callers
    void StartPlaying()
    {
        EventStartPlaying?.Invoke();
    }
    void StartPaused()
    {
        EventStartPaused?.Invoke();
    }
    void StartGameOver()
    {
        EventStartGameOver?.Invoke();
    }

    //Update event callers
    void StatePlaying(){
        EventPlaying?.Invoke();
    }
    void StatePaused(){
        EventPaused?.Invoke();
    }
    void StateGameOver(){
        EventGameOver?.Invoke();
    }

        //End event callers
    void EndPlaying()
    {
        EventEndPlaying?.Invoke();
    }
    void EndPaused()
    {
        EventEndPaused?.Invoke();
    }
    void EndGameOver()
    {
        EventEndGameOver?.Invoke();
    }

    public void QuitGame(){
        Debug.Log("Quitting game.");
        Application.Quit();
    }
}
