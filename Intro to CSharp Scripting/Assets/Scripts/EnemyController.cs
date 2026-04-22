using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 

public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Scanner
    }

    public EnemyType enemyType = EnemyType.Normal;


    [SerializeField] private float patrolDelay = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float patrolSpeed = 3;
    [SerializeField] private float scanRange = 5f;

    private Rigidbody2D _rb;
    private WaypointPath _waypointPath;
    private Vector2 _patrolTargetPosition;
    private Animator _animator;
    public Boolean seen = false;

    public void AcceptDefeat()
    {
        GameEventDispatcher.TriggerEnemyDefeated();
        Destroy(gameObject);
    }
    public void TakeHit()
    {
        _animator.Play("EnemyHit");
    }

    // Awake is called before Start
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _waypointPath = GetComponentInChildren<WaypointPath>();
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if (enemyType == EnemyType.Scanner)
        {
            yield break;
        }
        _patrolTargetPosition = _waypointPath.GetNextWaypointPosition();
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<HealthSystem>()?.Damage(3);
            Vector2 awayDirection = (Vector2)(other.transform.position - transform.position);
            other.transform.GetComponent<PlayerController>()?.Recoil(awayDirection * 3f);
        }
    }

    private void Update()
    {
        if (enemyType == EnemyType.Scanner)
        {
            if (!seen && GameManager.Instance.State == GameState.Playing)
            {
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            if(GameManager.Instance.State != GameState.Playing || seen)
            {
                transform.Rotate(0, 0, 0);
            } 

        }
    }

    private void FixedUpdate()
    {
        int layerMask = LayerMask.GetMask("Player");
        if (enemyType == EnemyType.Scanner)
        {
            Ray2D ray = new Ray2D(transform.position, transform.up);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, scanRange, layerMask);
            if (hit.collider)
            {
                seen = true;
                Attack(ray);
            }

            if (seen)
            {
                layerMask = LayerMask.GetMask("Default");
                hit = Physics2D.Raycast(transform.position, transform.up, scanRange, layerMask);
                if (hit.collider)
                {
                    _rb.velocity = Vector2.zero;
                    seen = false;
                    scanRange = 5f;
                }
                
            }
        }
        else
        {
            if (!_waypointPath) return;
            var dir = _patrolTargetPosition - (Vector2)transform.position;


            if (dir.magnitude <= 0.1)
            {
                //get next waypoint
                _patrolTargetPosition = _waypointPath.GetNextWaypointPosition();

                //change direction
                dir = _patrolTargetPosition - (Vector2)transform.position;
            }


            if (GameManager.Instance.State == GameState.Playing)
            {
                _rb.velocity = dir.normalized * patrolSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }


    }


    private void Attack(Ray2D ray)
    {
        scanRange = 3f;
        _rb.velocity = ray.direction * patrolSpeed;
    }
}
