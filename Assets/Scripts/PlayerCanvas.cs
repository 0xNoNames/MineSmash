using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Text percentage;
    [SerializeField] private Text wins;
    [SerializeField] private Image[] health;

    public void SetPlayerName(string _playerName)
    {
        playerName.text = _playerName;
    }

    public void SetPercentage(float _percentage)
    {
        percentage.text = _percentage.ToString("0.0") + "%";

        float delta = 0;
        if (_percentage > 0)
            delta = _percentage / 150;
        percentage.color = new Color(1, 1 - delta, 1 - delta);
    }

    public string GetPercentage()
    {
        return percentage.text;
    }

    public void SetHealth(int _health)
    {
        for (int i = 0; i < health.Length; i++)
        {
            if (_health > i)
                health[i].gameObject.SetActive(true);
            else
                health[i].gameObject.SetActive(false);
        }
    }
    public void SetWin(int _win)
    {
        wins.text = _win.ToString();
    }
}
