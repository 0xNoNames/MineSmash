using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerDetails> playerList = new List<PlayerDetails>();

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

    public void RestartGame()
    {
        foreach (PlayerDetails player in playerList)
        {
            player.Spawn();
            UIManager.Instance.getPlayerUI(player.playerID).SetHealth(player.maxHealth);
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            player.GetComponent<PlayerController>().SetDesactivateState(false, false);
        }
    }

    public void GameOver()
    {
        //List<int> playerHealths = new List<int>();

        if (playerList.Count <= 1)
            return;

        foreach (PlayerDetails player in playerList)
        {
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<PlayerController>().SetDesactivateState(true, true);

            UIManager.Instance.getPlayerUI(player.playerID).SetPercentage(0f);

            if (player.currentHealth > 0)
                UIManager.Instance.getPlayerUI(player.playerID).SetWin(player.wins);
        }
        RestartGame();
    }
}
