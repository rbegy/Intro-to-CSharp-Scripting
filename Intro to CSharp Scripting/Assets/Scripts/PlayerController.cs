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
    private bool _isRecoiling = false;
    private bool _isFireing = false;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();

        _rigidbody = GetComponent<Rigidbody2D>();

        //transform.position = new Vector2(3, -1);
        //Invoke(nameof(AcceptDefeat), 10);
    }

    public void AcceptDefeat()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.actions["Pause"].WasPressedThisFrame())
        {
            GameManager.Instance.TogglePause();
        }

        if (GameManager.Instance.State != GameState.Playing) return;

        if (_input.actions["Fire"].WasPressedThisFrame())
        {
            _isFireing = true;
            //Debug.Log("Fire activated!");
        }
    }

    public void Recoil(Vector2 directionVector)
    {
        _rigidbody.AddForce(directionVector, ForceMode2D.Impulse);
        _isRecoiling = true;
        Invoke(nameof(StopRecoiling), .3f);
    }

    private void StopRecoiling()
    {
        _isRecoiling = false;
    }

    private void FixedUpdate()
    {
        if (_isRecoiling) return;

        if (GameManager.Instance.State != GameState.Playing) return;

        var dir = _input.actions["Move"].ReadValue<Vector2>();

        _rigidbody.velocity = dir * 5;

        if (dir.magnitude > 0.5)
        {
            _facingVector = _rigidbody.velocity;
        }

        if (_isFireing)
        {
            var ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
            ball.GetComponent<BallController>()?.SetDirection(_facingVector);
            _isFireing = false;
        }
    }
}
