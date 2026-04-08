using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input; 
    private Rigidbody2D _rigidbody;
    private Vector2 _facingVector = Vector2.right;
    [SerializeField] private GameObject _ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();

        _rigidbody = GetComponent<Rigidbody2D>();

        //transform.position = new Vector2(3, -1);
        //Invoke(nameof(AcceptDefeat), 10);
    }

    void AcceptDefeat()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        if (_input.actions["Fire"].WasPressedThisFrame())
        {
            var ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);


            ball.GetComponent<Rigidbody2D>().velocity = Vector2.left * 10f;

            ball.GetComponent<Rigidbody2D>().velocity = _facingVector.normalized * 10f;
            //Debug.Log("Fire activated!");
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        var dir = _input.actions["Move"].ReadValue<Vector2>();

        _rigidbody.velocity = dir * 5;

        if (dir.magnitude > 0.5)
        {
            _facingVector = _rigidbody.velocity;
        }
    }
}
