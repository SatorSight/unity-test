using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMover : MonoBehaviour
{
    public float speed = 6;
    public float jumpAmount = 5;
    public Animator anim;
    private Vector3 direction;
    public FollowTarget lookPos;
    public Camera camera;

    // from 0 to 2, because there are 3 key items in the game
    public int inventory = 0;
    private float jumpStarts = 0f;
    private float groundingStarts = 0f;
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        //currentState = anim.GetCurrentAnimatorStateInfo(0);
        //previousState = currentState;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // basic movement vector, need to be separate without speed so we can normalize it
        Vector3 moveVec = new Vector3(horizontal, 0, vertical);
        moveVec.Normalize();

        // after normilize adjusting speed to make frame independent
        Vector3 spedUpVec = new Vector3(moveVec.x * speed * Time.deltaTime, 0, moveVec.z * speed * Time.deltaTime);
        // make it dependent on camera rotation
        Vector3 movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * spedUpVec;

        Quaternion camDirection = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        transform.Translate(movement, Space.World);

        // Here we need to rotate character with movement, but take in account that if we moving backwards rotation should be different
        if (movement != Vector3.zero)
        {

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            if (vertical > -0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            } else
            {
                toRotation = Quaternion.LookRotation(-movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            }
            
        }


        // This defines animations transitions
        anim.SetFloat("Speed", vertical);
        anim.SetFloat("Direction", horizontal);

        // inside we detect if jump key is pressed and if character can jump
        JumpWhenNeeded();
    }

    public void addToInventory()
    {
        inventory += 1;
    }

    private void JumpWhenNeeded()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanJump())
            {
                anim.SetBool("Jump", true);
                jumpStarts = Time.time;
                StartCoroutine(Jump());
            }
        }

        if (anim.GetBool("Jump"))
        {
            // because character is grounded by default, we can only detect if jump is over after detecting second grounding
            // so we check for grounding only while characher is midair
            if (Time.time > jumpStarts + 0.5f)
            {
                if (isGrounded())
                {
                    groundingStarts = Time.time;
                    anim.SetBool("Jump", false);
                }
            }
        }
    }

    private bool CanJump()
    {
        if (anim.GetBool("Jump") == true)
        {
            return false;
        }

        if (Time.time < groundingStarts + 0.2f)
        {
            return false;
        }

        return true;
    }

    // detect if character hits the ground, though being triggered two times: when jump starts and when it ends
    private bool isGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // needed for coroutine cause we need to delay the jump so animation matches the jump
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.15f);
        body.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
    }
}
