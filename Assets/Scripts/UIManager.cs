using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<PlayerCanvas> playerCanvasList = new List<PlayerCanvas>();
    public List<DummyCanvas> dummyCanvasList = new List<DummyCanvas>();

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
    }

    public PlayerCanvas GetPlayerUI(int p)
    {
        return playerCanvasList[p];
    }

    public DummyCanvas GetDummyUI(int p)
    {
        return dummyCanvasList[p];
    }
}