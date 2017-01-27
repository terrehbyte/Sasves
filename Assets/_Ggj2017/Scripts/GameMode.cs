using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UnityEventInt32 : UnityEvent<int> { }

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private Switcher playerSwitcher;

    public UnityEventInt32 onWinnerDetermined;

    [SerializeField]
    private Simon simon;

    [SerializeField]
    private Text timerLabel;

    [SerializeField]
    private Text roundLabel;

    private int roundCounter = 0;

    [SerializeField]
    public int maximumErrorThreshold = 3;

    [SerializeField]
    private ModalWindowView progressModal;

    [SerializeField]
    private Simon.Config[] roundConfigs;

    private enum RoundState
    {
        ENTER_BEFOREING,
        BEFOREING,

        ENTER_DISPLAYING,
        DISPLAYING,

        ENTER_REPLICATING,
        REPLICATING,

        ENTER_RESULTING,
        RESULTING,
    }

    private RoundState roundState = RoundState.ENTER_BEFOREING;

    void Update()
    {
        switch (roundState)
        {
            // BEFOREING
            case RoundState.ENTER_BEFOREING:
                EnterBeforeing();
                roundState = RoundState.BEFOREING;
                goto case RoundState.BEFOREING;
            case RoundState.BEFOREING:
                DoBeforeing();
                break;

            // DISPLAYING
            case RoundState.ENTER_DISPLAYING:
                EnterDisplaying();
                roundState = RoundState.DISPLAYING;
                goto case RoundState.DISPLAYING;
            case RoundState.DISPLAYING:
                DoDisplaying();
                break;

            // REPLICATING
            case RoundState.ENTER_REPLICATING:
                EnterReplicating();
                roundState = RoundState.REPLICATING;
                goto case RoundState.REPLICATING;
            case RoundState.REPLICATING:
                DoReplicating();
                break;

            // RESULTING
            case RoundState.ENTER_RESULTING:
                EnterResulting();
                roundState = RoundState.RESULTING;
                goto case RoundState.RESULTING;
            case RoundState.RESULTING:
                DoResulting();
                break;
        }
    }

    void EnterBeforeing()
    {
        // set Simon settings
        if (roundCounter < roundConfigs.Length - 1)
        {
            simon.gameConfig = roundConfigs[roundCounter];
        }
        else
        {
            // fallback
            simon.gameConfig.numObjects += 2;
        }
        simon.prepareRound();

        var playerImp = playerSwitcher.activeTarget.GetComponent<PlayersInput>();

        timerLabel.text = "...REMEMBER THIS...";
        playerImp.waveEndTimer.resetTimer();
        playerImp.waveEndTimer.setActive(false);
        roundLabel.text = "Round " + ++roundCounter;

        progressModal.gameObject.SetActive(false);
    }
    void DoBeforeing()
    {
        if (!simon.preparationTimer.isPassed())
            return;

        roundState = RoundState.DISPLAYING;
    }

    void EnterDisplaying()
    {

    }
    void DoDisplaying()
    {
        if (simon.isCurrentlyDisplaying)
            return;

        var playerImp = playerSwitcher.activeTarget.GetComponent<PlayersInput>();
        playerImp.IsFollowingSimon = true;

        roundState = RoundState.ENTER_REPLICATING;
        playerImp.waveEndTimer.setActive(true);
    }

    void EnterReplicating()
    {
    }
    void DoReplicating()
    {
        var playerImp = playerSwitcher.activeTarget.GetComponent<PlayersInput>();
        timerLabel.text = Mathf.CeilToInt(playerImp.waveEndTimer.timeRemaining()).ToString("D2");
        if (playerImp.isDone) { roundState = RoundState.ENTER_RESULTING; }
    }

    void EnterResulting()
    {
        var playerImp = playerSwitcher.activeTarget.GetComponent<PlayersInput>();
        playerImp.IsFollowingSimon = false;
        playerImp.onEndRound();
    }
    void DoResulting()
    {
        var playerImp = playerSwitcher.activeTarget.GetComponent<PlayersInput>();
        
        // display option to continue or retry if failed
        if(IsCompetent(playerImp))
        {
            // increase difficulty and provide positive reinforcement
            roundState = RoundState.ENTER_BEFOREING;
        }
        else
        {
            // prompt for reply
            progressModal.titleLabel.text = "Retry?";
            progressModal.questionLabel.text = "Would you like to retry?";
            progressModal.gameObject.SetActive(true);
        }
    }

    // Returns the player ID of the winning player.
    // Otherwise, returns -1 in the event of no winner.
    int GetWinnerID()
    {
        int theWinner = -1;
        int theWinnerCorrectness = 0;

        var players = playerSwitcher.targets;

        for (int i = 0; i < players.Count; ++i)
        {
            // do something to figure out the winner
            throw new System.NotImplementedException();

            // TODO: fill this in with the orders provided by the players
            int correctness = players[i].GetComponent<Simon>().CheckRight(null);

            if(correctness > theWinnerCorrectness)
            {
                theWinner = i;
                theWinnerCorrectness = correctness;
            }
        }

        if(theWinner != -1)
        {
            onWinnerDetermined.Invoke(theWinner);
        }

        return theWinner;
    }

    private bool IsCompetent(PlayersInput player)
    {
        return simon.CheckWrong(player.getPlayerInput()) < maximumErrorThreshold;
    }

    void Reset()
    {
        playerSwitcher = GetComponent<Switcher>();
    }
}