using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Movement movement { get; private set; }

    public EnemyHome home { get; private set; }

    public EnemyScatter scatter { get; private set; }

    public EnemyChase chase { get; private set; }

    public EnemyFrightened frightened { get; private set; }

    public EnemyBehavior initialBehavior;

    public Transform target;

    public int points = 200;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<EnemyHome>();
        this.scatter = GetComponent<EnemyScatter>();
        this.chase = GetComponent<EnemyChase>();
        this.frightened = GetComponent<EnemyFrightened>();
    }

    public void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        if (this.home != this.initialBehavior)
        {
            this.home.Disable();
        }

        if(this.initialBehavior != null)
        {
            this.initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().EnemyDefeated(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PlayerDefeated();
            }
        }
    }
}
