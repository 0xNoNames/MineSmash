using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<PlayerCanvas> playerCanvasList = new List<PlayerCanvas>();
    private static UIManager _instance;

    public static UIManager Instance
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

        foreach (PlayerCanvas playerUI in playerCanvasList)
        {
            playerUI.gameObject.SetActive(false);
        }
    }

    public PlayerCanvas getPlayerUI(int p)
    {
        return playerCanvasList[p];
    }
}