using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    
    private Rigidbody2D _rigidbody2D;

    private Stack<Command> _playerCommands;
    private Vector2 _moveDirection;

    private bool _isRecording;
    private bool _isPlaying;

    private float _recordTime;
    private Vector3 _recordPosition;
    private Quaternion _recordRotation;
    private Vector2 _recordVelocity;
    private float _recordTorque;

    private float _playTime;
    private Command[] _playCommands;
    private int _playIndex;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerCommands = new Stack<Command>();
    }

    public void RegisterJump(InputAction.CallbackContext context)
    {
        if (_isPlaying) return;
        
        if (context.performed)
        {
            _playerCommands.Push(new Jump(_rigidbody2D, jumpForce, Time.time));
           _playerCommands.Peek().Do();

           if (!_isRecording) _playerCommands.Pop();
        }
        
    }

    public void RegisterMove(InputAction.CallbackContext context)
    {
        if(_isPlaying) return;
        
        _playerCommands.Push(new Move(context.ReadValue<Vector2>(), this, Time.time));
        _playerCommands.Peek().Do();

        if (!_isRecording) _playerCommands.Pop();

    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(_moveDirection * (moveSpeed * Time.fixedDeltaTime));
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
    }

    private void Update()
    {
        if (_isPlaying)
        {
            if (_playCommands[_playIndex].time - _recordTime <= Time.time - _playTime)
            {
                _playCommands[_playIndex].Do();
                _playIndex--;

                if (_playIndex < 0)
                {
                    _isPlaying = false;
                }
            }
        }
        
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if(_isPlaying) return;
            
            if (!_isRecording)
            {
                _recordTime = Time.time;
                _recordPosition = transform.position;
                _recordRotation = transform.rotation;
                _recordVelocity = _rigidbody2D.velocity;
                _recordTorque = _rigidbody2D.totalTorque;
                _isRecording = true;
            }
            else
            {
                _isRecording = false;
            }
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if(_isRecording) return;

            if (!_isPlaying)
            {
                _playTime = Time.time;
                _playCommands = _playerCommands.ToArray();
                _playIndex = _playCommands.Length - 1;
                _isPlaying = true;
                
                transform.position =  _recordPosition;
                transform.rotation = _recordRotation;
                _rigidbody2D.velocity = _recordVelocity;
                _rigidbody2D.totalTorque = _recordTorque;
            }
        }
    }
}

public abstract class Command
{
    public float time;

    public Command(float tim)
    {
        time = tim;
    }
    public abstract void Do();
    public abstract void Undo();
}

public class Jump : Command
{
    private Rigidbody2D _rigidbody2D;
    private float jumpForce;


    public Jump(Rigidbody2D rb2d, float jump, float tim) : base(tim)
    {
        _rigidbody2D = rb2d;
        jumpForce = jump;

    }
    
    public override void Do()
    {
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public override void Undo()
    {
        
    }
}

public class Move : Command
{
    private Vector2 direction;
    private PlayerController player;

    public Move(Vector2 dir, PlayerController play, float tim) : base(tim)
    {
        direction = dir;
        player = play;
    }
    public override void Do()
    {
        player.SetMoveDirection(direction);
    }

    public override void Undo()
    {
        
    }
}
