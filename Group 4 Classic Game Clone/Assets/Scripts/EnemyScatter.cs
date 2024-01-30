using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScatter : EnemyBehavior
{
    private void OnDisable()
    {
        this.enemy.chase.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.enemy.frightened.enabled)
        {
            int index = Random.Range(0, node.availableDirections.Count);

            if (node.availableDirections[index] == -this.enemy.movement.direction && node.availableDirections.Count > 1)
            {
                index++;

                if(index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            this.enemy.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
