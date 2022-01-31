using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerDetails> playerList = new List<PlayerDetails>();

    public GameObject dummy;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public PlayerDetails getPlayerDetails(int p)
    {
        return playerList[p];
    }

    public IEnumerator RestartGame()
    {
        foreach (PlayerDetails player in playerList)
        {
            player.Spawn();

            yield return new WaitForSeconds(1f);

            player.isDead = false;
            player.playerController.SetDesactivateState(false);
        }
    }
    public void GameOver()
    {
        //List<int> playerHealths = new List<int>();

        foreach (PlayerDetails player in playerList)
        {
            player.playerController.SetDesactivateState(true);

            UIManager.Instance.GetPlayerUI(player.playerID).SetPercentage(0f);
            UIManager.Instance.GetPlayerUI(player.playerID).SetHealth(player.maxHealth);

            if (player.currentHealth > 0)
            {
                player.wins += 1;
                UIManager.Instance.GetPlayerUI(player.playerID).SetWin(player.wins);
            }

            player.ResetPlayer();
        }

        StopCoroutine("RestartGame");
        StartCoroutine("RestartGame");
    }

}
