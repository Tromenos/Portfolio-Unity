using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Camera cam = null;

    [SerializeField]
    private Rigidbody rb = null;

    [Header("Variables")]
    [SerializeField]
    private float jumpStrength = 275.0f;

    [SerializeField]
    private float rotationSpeed = 80.0f;

    [SerializeField]
    private float maxRotDegree = 75.0f;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float sprintModifier = 2.0f;

    [SerializeField]
    private int collisionLayer = 0;

    [SerializeField]
    private LayerMask groundingLayer = 0;

    [SerializeField]
    private Vector3 topOffset = new Vector3();

    [SerializeField]
    private Vector3 bottomOffset = new Vector3();

    [SerializeField]
    private Vector3 leftOffset = new Vector3();

    [SerializeField]
    private Vector3 rightOffset = new Vector3();


    private Vector3 _moveDir     = new Vector3();
    private bool    _isGrounded  = true;
    private bool    _isSprinting = false;

    private void Awake()
    {
        if (cam == null || rb == null)
            this.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        RotatePlayer(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        MovePlayer(Time.deltaTime);
    }

    private void MovePlayer(float dt)
    {
        if (Input.GetKey(KeyCode.W))
            _moveDir += rb.transform.forward;
        if (Input.GetKey(KeyCode.S))
            _moveDir -= rb.transform.forward;
        if (Input.GetKey(KeyCode.A))
            _moveDir -= rb.transform.right;
        if (Input.GetKey(KeyCode.D))
            _moveDir += rb.transform.right;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _isSprinting = false;
        if (_isGrounded && Input.GetKey(KeyCode.Space))
        {
            _isGrounded = false;
            rb.AddForce(Vector3.up * jumpStrength);
        }

        var moveVector = _moveDir;

        if (_isSprinting)
            moveVector *= (moveSpeed * sprintModifier * dt);
        else
            moveVector *= (moveSpeed * dt);

        if (CanMove(moveVector))
            rb.transform.position += moveVector;
        _moveDir.Set(0.0f, 0.0f, 0.0f);
    }

    private bool CanMove(Vector3 move)
    {
        float length    = move.magnitude;
        var   pos       = rb.transform.position;
        var   direction = move.normalized;
        return !Physics.Raycast(pos, direction, length, collisionLayer)                &&
               !Physics.Raycast(pos + topOffset, direction, length, collisionLayer)    &&
               !Physics.Raycast(pos + bottomOffset, direction, length, collisionLayer) &&
               !Physics.Raycast(pos + leftOffset, direction, length, collisionLayer)   &&
               !Physics.Raycast(pos + rightOffset, direction, length, collisionLayer);
    }

    private void RotatePlayer(float dt)
    {
        float rotY = CalcRotY();
        rb.transform.eulerAngles += new Vector3(0.0f, rotY * dt);

        var   camTransform = cam.transform;
        float rotX         = CalcRotX(camTransform.eulerAngles.x);

        camTransform.eulerAngles += new Vector3(rotX * dt, 0.0f);
    }

    private float CalcRotX(float currentEulerX)
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float rotX   = 0.0f;

        if (mouseY <= -0.1f || mouseY >= 0.1f)
            rotX = -mouseY;

        float newX = currentEulerX + rotX;

        if (newX > maxRotDegree && newX < (360 - maxRotDegree))
            return 0.0f;

        return rotX * rotationSpeed;
    }

    private float CalcRotY()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (mouseX <= -0.1f || mouseX >= 0.1f)
            return mouseX * rotationSpeed;

        return 0.0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_isGrounded && other.gameObject.layer == (int) (Mathf.Log(groundingLayer.value, 2)))
            _isGrounded = true;
    }

    // private void OnValidate()
    // {
    //     collisionLayer = 1 << collisionLayer;
    // }
}