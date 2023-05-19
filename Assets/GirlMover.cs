using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMover : MonoBehaviour
{

    public float speed = 6;
    public float jumpAmount = 10;
    public Animator anim;
    private AnimatorStateInfo currentState;     // 現在のステート状態を保存する参照
    private AnimatorStateInfo previousState;    // ひとつ前のステート状態を保存する参照
    private Vector3 direction;
    public FollowTarget lookPos;
    public Camera camera;
    private float jumpStarts;


    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;
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

        // I dont know what this code does and how its used

        // ↑キー/スペースが押されたら、ステートを次に送る処理
        if (Input.GetKeyDown("up") || Input.GetButton("Jump"))
        {
            // ブーリアンNextをtrueにする
            anim.SetBool("Next", true);
        }

        // ↓キーが押されたら、ステートを前に戻す処理
        if (Input.GetKeyDown("down"))
        {
            // ブーリアンBackをtrueにする
            anim.SetBool("Back", true);
        }

        // "Next"フラグがtrueの時の処理
        if (anim.GetBool("Next"))
        {
            // 現在のステートをチェックし、ステート名が違っていたらブーリアンをfalseに戻す
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (previousState.nameHash != currentState.nameHash)
            {
                anim.SetBool("Next", false);
                previousState = currentState;
            }
        }

        // "Back"フラグがtrueの時の処理
        if (anim.GetBool("Back"))
        {
            // 現在のステートをチェックし、ステート名が違っていたらブーリアンをfalseに戻す
            currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (previousState.nameHash != currentState.nameHash)
            {
                anim.SetBool("Back", false);
                previousState = currentState;
            }
        }

        // end of code I didn't write


        // This defines animations transitions
        anim.SetFloat("Speed", vertical);
        anim.SetFloat("Direction", horizontal);

        // inside we detect if jump key is pressed and if character can jump
        JumpWhenNeeded();

        if (isGrounded())
        {
            Debug.Log("isGrounded()");
            Debug.Log(isGrounded());
        }

    }


    private void JumpWhenNeeded()
    {

        if (Time.time > jumpStarts + 0.5f)
        {

            if (isGrounded())
            {
                anim.SetBool("Jump", false);
            }


        }

        bool isJumping = anim.GetBool("Jump");

        if (isJumping == false)
        {
            

            if (Input.GetKeyDown(KeyCode.Space) && anim.GetBool("Jump") == false)
            {
                anim.SetBool("Jump", true);
                StartCoroutine(Jump());
            }
        }

        
    }

    // detect if character hits the ground, though being triggered two times: when jump starts and when it ends
    public bool isGrounded()
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

        //code for starting the animation, however you are doing it
        

        //leave code and come back after a second
        yield return new WaitForSeconds(0.15f);

        //your code for jumping
        body.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
        jumpStarts = Time.time;
    }
}
