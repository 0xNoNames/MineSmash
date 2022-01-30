using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpSystem : MonoBehaviour
{
    public Vector2 value;

    private void FixedUpdate()
    {
        value.x = Mathf.Lerp(value.x, 0, Time.fixedDeltaTime * 3f);
        value.y = Mathf.Lerp(value.y, 0, Time.fixedDeltaTime * 5f);

        if (Mathf.Abs(value.x) < 0.1)
            value.x = 0;

        if (Mathf.Abs(value.y) < 3)
            value.y = 0;
    }
}
