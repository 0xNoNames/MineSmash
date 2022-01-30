using System.Collections;
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

    public IEnumerator RestartGame()
    {
        foreach (PlayerDetails player in playerList)
        {
            player.Spawn();

            UIManager.Instance.getPlayerUI(player.playerID).SetPercentage(0f);
            UIManager.Instance.getPlayerUI(player.playerID).SetHealth(player.maxHealth);


            yield return new WaitForSeconds(1f);

            player.playerController.SetDesactivateState(false, false);
        }
    }
    public void GameOver(PlayerDetails _player)
    {
        //List<int> playerHealths = new List<int>();

        foreach (PlayerDetails player in playerList)
        {
            player.playerController.SetDesactivateState(true, true);

            _player.ResetPlayer();

            if (player.currentHealth > 0)
                UIManager.Instance.getPlayerUI(player.playerID).SetWin(player.wins);
        }

        StopCoroutine("RestartGame");
        StartCoroutine("RestartGame");
    }

}
