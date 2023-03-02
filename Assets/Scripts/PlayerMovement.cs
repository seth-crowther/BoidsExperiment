using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 10;
    private CharacterController controller;

    private const float gravity = 0.2f;
    private const float jumpSpeed = 0.075f;
    private float vSpeed = 0.0f;

    private bool isGrounded = false;

    [SerializeField] private float slopeRayLength;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float MoveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float MoveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3[] moveVectors = GetMovementVectors();
        Vector3 velocity = MoveX * moveVectors[1] + MoveY * moveVectors[0];

        if (isGrounded)
        {
            if (Input.GetAxis("Jump") > 0)
            {
                vSpeed = jumpSpeed;
                velocity.y += vSpeed;
            }
            else
            {
                vSpeed = 0;
            }
        }
        else
        {
            vSpeed -= gravity * Time.deltaTime;
            Vector3 vVel = vSpeed * Vector3.up;
            velocity += vVel;
        }

        controller.Move(velocity);
    }

    private Vector3[] GetMovementVectors()
    {
        Vector3[] returnTuple = new Vector3[2] { transform.forward, transform.right };

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height / 2 * slopeRayLength))
        {
            returnTuple[0] = Vector3.Cross(transform.right, hit.normal);
            returnTuple[1] = Vector3.Cross(-transform.forward, hit.normal);

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        return returnTuple;
    }
}
