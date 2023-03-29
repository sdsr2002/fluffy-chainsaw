using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    // Ball Variables
    [Header("Ball Variables")]

    /// <summary>
    /// How many seconds after launch until it will detach the springjoint
    /// </summary>
    [SerializeField, Tooltip("How fast the ball detaches from springjoint")] 
    private float _detachDelay = 0.2f;

    [SerializeField,Tooltip("How fast the ball will respawn in seconds after first ball was launched")]
    private float _respawnDelay = 0.5f;

    /// <summary>
    /// how many seconds after launch it will despawn
    /// </summary>
    [SerializeField, Tooltip("despawning Timer")] 
    private float _ballDespawnDelay = 10f;
    [Space]

    /// <summary>
    /// Ball to be spawned
    /// </summary>
    [SerializeField, Tooltip("Ball to be spawned")]
    private GameObject _ballPrefab;

    /// <summary>
    /// springJoints connected body and spawnpoint for the balls
    /// </summary>
    [SerializeField, Tooltip("The anchor the springjoint will spring towards. also where new balls will spawn")]
    private Rigidbody2D _anchor;

    [Space]
    [Header("Debug Values")]
    [SerializeField]
    private Ball _currentBall;
    
    //
    // Camera Variables
    //

    private Camera _camera;
    private Camera _mainCamera
    {
        set
        {
            _camera = value;
        }
        get
        {
            if (!_camera) _mainCamera = Camera.main;
            return _camera;
        }
    }

    //
    // Input variables
    //

    private Vector2 _touchPosition = Vector2.zero;
    private bool _isDragging = false;

    //
    // Methods
    //

    private void Awake()
    {
        SpawnNewBall();

    }

    private void Update()
    {
        if (!_currentBall) return;
        // checks for touch
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (_isDragging)
            {
                // After stoped dragging launch the ball
                LaunchBall();
                _isDragging = false;
            }
            return;
        }

        // change state to dragging
        _isDragging = true;

        // Get Inputs
        GetInputs();

        // set touch position to Ball
        MoveBall();

    }

    /// <summary>
    /// Will take your view position and convert it to world position (- Camera Z position)
    /// </summary>
    /// <param name="ScreenPosition">View Position</param>
    /// <returns>World Position</returns>
    private Vector3 ViewToWorldPosition(Vector2 ScreenPosition)
    {
        return _mainCamera.ScreenToWorldPoint(ScreenPosition) - _mainCamera.transform.position.z * Vector3.forward;
    }

    /// <summary>
    /// Gets the mouse/touch Position on screen
    /// </summary>
    private void GetInputs()
    {
        _currentBall.IsKinematic = true;
        _touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
    }

    /// <summary>
    /// Moves the ball to Mouse/Touch on the screen in world position
    /// </summary>
    private void MoveBall()
    {
        _currentBall.Position = ViewToWorldPosition(_touchPosition);
    }

    /// <summary>
    /// Spawns new ball and assignes necessary variables
    /// </summary>
    private void SpawnNewBall()
    {
        GameObject newball = Instantiate(_ballPrefab, _anchor.transform.position, _ballPrefab.transform.rotation);
        if(newball.TryGetComponent<Ball>(out _currentBall))
        {
            _currentBall.JointConnectedBody = _anchor;
        }
        else
        {
            Destroy(newball);
            Debug.LogError($"No '{nameof(Ball)}' script was found on Instatiated ball with Prefab:{_ballPrefab.gameObject.name}");
        }
    }

    /// <summary>
    /// Launches the ball and detaches fully from this script after "_detachDelay" in seconds
    /// </summary>
    private void LaunchBall()
    {
        // Launch
        _currentBall.IsKinematic = false;

        // Detach after "_detachDelay"
        Invoke(nameof(DetachBall), _detachDelay);
    }

    /// <summary>
    /// Detaches the ball from joints and this script
    /// </summary>
    private void DetachBall()
    {
        // Detach
        _currentBall.SpringjointEnable = false;
        // Despawn after x seconds
        _currentBall.Despawn(_ballDespawnDelay);
        // Forget ball
        _currentBall = null;
        // Create new after x seconds
        Invoke(nameof(SpawnNewBall),_respawnDelay);
    }

}
