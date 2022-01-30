using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpSystem : MonoBehaviour
{
    public Vector2 bump;

    private void FixedUpdate()
    {
        bump.x = Mathf.Lerp(bump.x, 0, Time.fixedDeltaTime * 10f);
        bump.y = Mathf.Lerp(bump.y, 0, Time.fixedDeltaTime * 10f);
    }
}
