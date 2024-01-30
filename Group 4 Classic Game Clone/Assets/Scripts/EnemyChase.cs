using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBehavior
{
        private void OnDisable()
    {
        this.enemy.scatter.Enable();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.enemy.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;

            float minimumDistance = float.MaxValue;

            foreach(Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                
                float distance = (this.enemy.target.position - newPosition).sqrMagnitude;

                if (distance < minimumDistance)
                {
                    direction = availableDirection;
                    minimumDistance = distance;
                }
            }

            this.enemy.movement.SetDirection(direction);
        }
    }
}
