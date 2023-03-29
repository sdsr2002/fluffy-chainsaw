using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpringJoint2D _springJoint;

    private void Awake()
    {
        VariableCheck();
    }

    /// <summary>
    /// Check if every Variable that should exist exists otherwise show error message
    /// </summary>
    private void VariableCheck()
    {
        if (!_rigidbody)
        {
            if (!TryGetComponent<Rigidbody2D>(out _rigidbody))
            {
                Debug.LogError(gameObject.name + " has no rigidbody2D");
            }
        }
        if (!_springJoint)
        {
            if (!TryGetComponent<SpringJoint2D>(out _springJoint))
            {
                Debug.LogError(gameObject.name + " has no SpringJoint2D");
            }
        }
    }

    /// <summary>
    /// The Connected body to the ball Springjoint
    /// </summary>
    public Rigidbody2D JointConnectedBody
    {
        set => _springJoint.connectedBody = value;
        get => _springJoint.connectedBody;
    }

    /// <summary>
    /// Ball Position
    /// </summary>
    public Vector3 Position 
    {
        set => transform.position = value;
        get => transform.position;
    }

    /// <summary>
    /// SpringJoint2D Enable State
    /// </summary>
    public bool SpringjointEnable
    {
        set
        {
            _springJoint.enabled = value;
        }
        get
        {
            return _springJoint.enabled;
        }
    }


    /// <summary>
    /// Balls rigidbody kinematic state
    /// </summary>
    public bool IsKinematic 
    {
        set { if (_rigidbody.isKinematic != value) _rigidbody.isKinematic = value; }
        get => _rigidbody.isKinematic;
    }

    /// <summary>
    /// Destroys object after x seconds
    /// </summary>
    /// <param name="time">seconds</param>
    public void Despawn(float time)
    {
        Invoke(nameof(Despawn),time);
    }
    private void Despawn()
    {
        Destroy(gameObject);
    }
}
