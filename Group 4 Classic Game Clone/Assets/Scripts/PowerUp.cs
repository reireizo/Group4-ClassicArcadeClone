using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Junk
{
    public float duration = 8.0f;

    protected override void Collect()
    {
        FindObjectOfType<GameManager>().PowerUpCollected(this);
    }
}
