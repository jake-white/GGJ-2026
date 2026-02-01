using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SequenceManager : SingletonBehavior<SequenceManager>
{
    public enum GameState { Ready, Bombing, Bossing, Landing, Finished };
    public List<TrackedHoop> hoopsToFlyThrough;
    public List<TrackedCrater> cratersToDestroy;
    public TextMeshProUGUI timer;
    public bool debug = false;
    float timeStarted, elapsedTime, finalTime;
    private GameState state = GameState.Ready;
    float instructions0Played = -30, instructions1Played = -30;
    float score;
    public void StartGame()
    {
        timeStarted = Time.time;
        if(Drone.Instance.state == Drone.FlightState.Landed || debug)
        {
            Debug.Log("Starting game!");
            state = GameState.Bombing;

            foreach (TrackedHoop hoop in hoopsToFlyThrough)
            {
                hoop.ToggleLight(true);
            }

            foreach (TrackedCrater crater in cratersToDestroy)
            {
                crater.ToggleLight(false);
            }
            AudioManager.Instance.QueueCommanderVoice(5);
        }
    }

    private void Update()
    {
        if (state != GameState.Ready && state != GameState.Finished)
        {
            elapsedTime = Time.time - timeStarted;
            elapsedTime -= score;
            System.TimeSpan span = System.TimeSpan.FromSeconds(elapsedTime);
            timer.text = span.ToString(@"mm\:ss");
        }
        if(state == GameState.Landing && Drone.Instance.state == Drone.FlightState.Landed)
        {
            state = GameState.Finished;
            finalTime = elapsedTime;
            Debug.Log("Finished with final time of " + timer.text);
        }
        if(Time.time - instructions0Played > 30 && state == GameState.Bombing && Drone.Instance.state == Drone.FlightState.Flying)
        {
            instructions0Played = Time.time;
            AudioManager.Instance.QueueCommanderVoice(1);
        }
        else if (Time.time - instructions1Played > 30 && state == GameState.Bossing)
        {
            instructions1Played = Time.time;
            AudioManager.Instance.QueueCommanderVoice(2);
        }
    }

    public void CraterDestroyed(TrackedCrater crater)
    {
        cratersToDestroy.Remove(crater);
        if(cratersToDestroy.Count == 0)
        {
            state = GameState.Bossing;
            Mothership.Instance.BreakShields();
        }
    }

    public void MothershipDestroyed()
    {
        state = GameState.Landing;
    }

    public void EnemyDefeated()
    {
        score++;
    }
}
