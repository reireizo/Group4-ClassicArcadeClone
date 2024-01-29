using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{
    public int points = 10;

    protected virtual void Collect()
    {
        FindObjectOfType<GameManager>().JunkCollected(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Collect();
        }
    }
}
