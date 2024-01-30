using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHome : EnemyBehavior
{
    public Transform insideHome;

    public Transform leave;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if(this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            this.enemy.movement.SetDirection(-this.enemy.movement.direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        this.enemy.movement.SetDirection(Vector2.up, true);
        this.enemy.movement.rigidbody.isKinematic = true;
        this.enemy.movement.enabled = false;

        Vector3 position = this.transform.position;

        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.insideHome.position, elapsed / duration);
            newPosition.z = position.z;
            this.enemy.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(this.insideHome.position, this.leave.position, elapsed / duration);
            newPosition.z = position.z;
            this.enemy.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.enemy.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        this.enemy.movement.rigidbody.isKinematic = false;
        this.enemy.movement.enabled = true;
    }
}
