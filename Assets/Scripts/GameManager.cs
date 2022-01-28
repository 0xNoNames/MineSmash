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

    public void RestartGame()
    {
        foreach (PlayerDetails player in playerList)
        {
            player.Spawn();
        }
    }

    public void GameOver(PlayerDetails winner, PlayerDetails looser)
    {
        winner.GetComponent<Rigidbody2D>().isKinematic = true;
        looser.GetComponent<Rigidbody2D>().isKinematic = true;

        // Ajouter +1 sur le nombre de win au winner
        RestartGame();
    }
}
