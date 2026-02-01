using CollabXR;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : SingletonBehavior<SequenceManager>
{
    public enum GameState { Ready, Bombing, Bossing, Landing };
    public List<TrackedCrater> cratersToDestroy;
    private GameState state = GameState.Ready;
    public void StartGame()
    {
        if(Drone.Instance.state == Drone.FlightState.Landed)
        {
            state = GameState.Bombing;
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
        state = GameState.Landing;
    }
}
