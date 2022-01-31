using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCanvas : MonoBehaviour
{
    [SerializeField] private Text dummyName;
    [SerializeField] private Text percentage;

    public void SetDummyName(string _dummyName)
    {
        dummyName.text = _dummyName;
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
}
