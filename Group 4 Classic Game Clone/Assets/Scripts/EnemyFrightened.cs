using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrightened : EnemyBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer moveIndicator;
    public SpriteRenderer afraid;
    public SpriteRenderer flashing;

    public bool defeated { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.moveIndicator.enabled = false;
        this.afraid.enabled = true;
        this.flashing.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.moveIndicator.enabled = true;
        this.afraid.enabled = false;
        this.flashing.enabled = false;
    }

    private void Flash()
    {
        if (!this.defeated)
        {
            this.afraid.enabled = false;
            this.flashing.enabled = true;
            this.flashing.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Defeated()
    {
        this.defeated = true;

        Vector3 position = this.enemy.home.insideHome.position;
        position.z = this.enemy.transform.position.z;
        this.enemy.transform.position = position;

        this.enemy.home.Enable(this.duration);

        this.body.enabled = false;
        this.moveIndicator.enabled = true;
        this.afraid.enabled = false;
        this.flashing.enabled = false;
    }

    private void OnEnable()
    {
        this.enemy.movement.speedFactor = 0.5f;
        this.defeated = false;
    }

    private void OnDisable()
    {
        this.enemy.movement.speedFactor = 1.0f;
        this.defeated = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (this.enabled)
            {
                Defeated();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;

            float maxDistance = float.MinValue;

            foreach(Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                
                float distance = (this.enemy.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            this.enemy.movement.SetDirection(direction);
        }
    }
}
