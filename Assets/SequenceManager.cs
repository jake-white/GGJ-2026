using CollabXR;
using UnityEngine;

public class SequenceManager : SingletonBehavior<SequenceManager>
{
    public enum GameState { Ready, Bombing, Bossing, Landing };
    private GameState state = GameState.Ready;
    public void StartGame()
    {
        if(Drone.Instance.state == Drone.FlightState.Landed)
        {
            state = GameState.Bombing;
        }
    }
}
