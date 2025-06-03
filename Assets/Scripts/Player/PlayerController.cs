using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     * The rights to 
     * use and modernize the code 
     * belong to NoName Studio Inc (c)
    */

    [Header("Movement")]
    private const float speed = 6;
    private const float jumpingPower = 6;
    private const float gravity = 9.81f; //S!57gL3
    public Vector3 moveVector = new Vector3(0, 0, 0);

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    
    private CharacterController characterController;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Blockers")]
    private bool[] blockMove = new bool[3] { false, false, false };

    [Header("Settings")]
    [SerializeField] private BoxCollider hitBox; //S!57gL3
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groungLayer;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        /*
         * Apply Move Vector
        */
        if((
                (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) 
                || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            ) && moveVector.x != 0
        )
            moveVector.x = 0;
        else if(!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveVector.x = 1;
        else if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) moveVector.x = -1;
        
        if((
                (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) 
                || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            ) && moveVector.z != 0
        ) 
            moveVector.z = 0;
        else if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) moveVector.z = 1;
        else if(!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVector.z = -1;

        /*
         * Jumping System
        */
        if(Input.GetKeyDown(KeyCode.Space) && IsGround()) {
            rigidBody.velocity = new Vector3(
                rigidBody.velocity.x, 
                !blockMove[1] ? jumpingPower : rigidBody.velocity.y, 
                rigidBody.velocity.z
            );
        }

        /*
         * Handle Movement
        */
        rigidBody.velocity = new Vector3(
            !blockMove[0] ? moveVector.x * speed : rigidBody.velocity.x, 
            rigidBody.velocity.y, 
            !blockMove[2] ? moveVector.z * speed : rigidBody.velocity.z
        );

        /*
         * Dashing
        */
        /* Handle dash cooldown */
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

        /* Handle dash input */
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (cooldownTimer <= 0 && !isDashing) {
                if (Mathf.Abs(moveVector.x) > 0.1f || 
                Mathf.Abs(moveVector.z) > 0.1f) 
                    dashDirection = new Vector3(moveVector.x, 0, moveVector.z).normalized;
                else dashDirection = transform.forward;

                /* Start dash */
                isDashing = true;
                dashTimer = dashDuration;
                cooldownTimer = dashCooldown;
            }
        }

        /* Process dash */
        if (isDashing) {
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0) isDashing = false;
            else {
                float dashSpeed = dashDistance / dashDuration;
                rigidBody.velocity = dashDirection * dashSpeed * Time.deltaTime;
            }
        }
    }

    private bool IsGround() { return Physics.CheckSphere(groundCheck.position, 0.2f, groungLayer); }
}
