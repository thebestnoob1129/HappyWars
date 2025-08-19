using UnityEngine;

public class Tower : Machine
{
/// <summary>
///
/// Tower Object might be changed to become center of tycoon
/// Tower Object might be used to control the tycoon and be health of the tycoon
///
/// </summary>
    private int claimedTeam = -1;
    private int control = 0;

    private Tycoon controlTycoon;
    
    private void FixedUpdate()
    {
        GameUpdate();
        // Check if the tower is being controlled by a team
        if (claimedTeam != -1)
        {
            // Logic for controlling the tower can be added here
            foreach (var tycoon in FindObjectsOfType<Tycoon>())
            {
                if (tycoon.team == claimedTeam)
                {
                    // Perform actions related to the team controlling the tower
                    //tycoon.AddControlTower(this);
                    controlTycoon = tycoon;
                }
            }

            Debug.Log("Tower controlled by team: " + claimedTeam);
        }

    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            Player player  = other.gameObject.GetComponent<Player>();
            if (claimedTeam == -1 || claimedTeam == player.Team)
            {
                claimedTeam = player.Team;
                control++;
                Debug.Log("Tower claimed by team: " + claimedTeam);
                // Additional logic for claiming the tower can be added here
            }
            else
            {
                Debug.LogWarning("Tower already claimed by another team.");
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (claimedTeam == player.Team)
            {
                control = 0;
                Debug.Log("Tower released by team: " + player.Team);
            }
        }
    }
}