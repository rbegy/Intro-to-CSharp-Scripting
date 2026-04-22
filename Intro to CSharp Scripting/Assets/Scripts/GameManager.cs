using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//An enum of all the possible GameStates (Many are Gameplay Modes!)
[Serializable]
public enum GameState
{
    Starting = 1,
    Playing = 10,
    Paused = 15,
    FailScreen = 20,
    VictoryDance = 25
}

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public GameState State { get; private set; }

    private GameState _previousState = GameState.Starting;
    public Text text;


    void Start()
    {
        //Begin with the "Starting" game state
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        _previousState = State;
        State = newState;
        Debug.Log("Changed Game State to    : " + newState);
        

        //This Game Manager can do high level manager stuff, itself.
        switch (newState)
        {
            case GameState.Starting:
                Time.timeScale = 1;
                StartCoroutine(HandleStarting());
                break;
            case GameState.Playing:
                text.text = "";
                Time.timeScale = 1;
                break;
            case GameState.Paused:
                text.text = "PAUSED";
                Time.timeScale = 0;
                break;
            case GameState.FailScreen:
                StartCoroutine(Death());
                break;
            case GameState.VictoryDance:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    public void ToggleDeath()
    {
        ChangeState(GameState.FailScreen);
    }

    public void TogglePause()
    {
        //ChangeState toggles between Paused and previous state
        if (State == GameState.Paused)
            ChangeState(_previousState);
        else
            ChangeState(GameState.Paused);
    }

    public IEnumerator Death()
    {
        text.text = "You Died";
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HandleStarting()
    {
        text.text = "3";
        yield return new WaitForSeconds(1);
        text.text = "2";
        yield return new WaitForSeconds(1);
        text.text = "1";
        yield return new WaitForSeconds(1);
        text.text = "GO";
        yield return new WaitForSeconds(.5f);
        ChangeState(GameState.Playing);
    }
}