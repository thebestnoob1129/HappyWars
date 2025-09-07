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

    private float health;
    public float Health => health;
    private float maxHealth;

    void Start()
    {
        maxHealth = this.Value;
        health = maxHealth;

    }
    
    private void FixedUpdate()
    {
        GameUpdate();

        this.SetDisplayText(health.ToString());

        // Check if the tower is being controlled by a team
        /* Mine Activity NOT TOWER
        if (claimedTeam != -1)
        {
            // Logic for controlling the tower can be added here
            foreach (var tycoon in FindObjectsOfType<Tycoon>())
            {
                if (tycoon.Team == claimedTeam)
                {
                    // Perform actions related to the team controlling the tower
                    //tycoon.AddControlTower(this);
                    controlTycoon = tycoon;
                }
            }

            Debug.Log("Tower controlled by team: " + claimedTeam);
        }
        */

    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // if player hits tower remove health and destroy
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