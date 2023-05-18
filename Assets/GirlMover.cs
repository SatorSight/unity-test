﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMover : MonoBehaviour
{

    public float speed = 6;
    public float turnSpeed = 2;
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
        //lookPos = GetComponent<FollowTarget>();


    }

    void Start()
    {
        // 各参照の初期化
        //anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;
        camera = Camera.main;





    }

// Update is called once per frame
void Update()
    {

        //PreventChildRotation();

        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
        //           Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);


        //transform.center = Camera.main.transform.position;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(horizontal, 0, vertical);
        moveVec.Normalize();

        Vector3 spedUpVec = new Vector3(moveVec.x * speed * Time.deltaTime, 0, moveVec.z * speed * Time.deltaTime);

        Vector3 movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * spedUpVec;
        //movement.Normalize();

        Quaternion camDirection = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);


        //Vector3 anotherVec = new Vector3(horizontal , 0, vertical);
        //anotherVec.Normalize();
        //Vector3 someVec = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        //Vector3 movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * someVec;





        //transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);

        transform.Translate(movement, Space.World);

        //Vector3 movement2 = new Vector3(movement.x, movement.y, movement.z);

        if (movement != Vector3.zero)
        {

            //Debug.Log("movement");
            //Debug.Log(movement);
            //Debug.Log("camera.transform.forward");
            //Debug.Log(camera.transform.forward);

            ////Vector3 fwd = transform.forward;

            //// use the 0.5f through arccos for the 30-degree demarcation
            //// angle past which we will consider you moving backwards.
            //if (Vector3.Dot(movement, camera.transform.forward) < -0.5f)
            //{
            //    movement = -movement;
            //    // walking backwards
            //    //gameObject.transform.forward = -move;
            //}

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            if (vertical > -0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            } else
            {
                toRotation = Quaternion.LookRotation(-movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            }


            //transform.forward = movement;
            
        }

        //float turn = Input.GetAxis("Mouse X");


        //direction = new Vector3(horizontal, 0, vertical);

        //direction = direction.normalized;
        //body.velocity = Camera.main.transform.TransformDirection(direction) * speed * Time.deltaTime;




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

        /* if (speed > 0) {
             if (horizontal > 0.1)
             {
                 anim.SetFloat("Direction", 1);
             }
             else if (horizontal < -0.1)
             {
                 anim.SetFloat("Direction", -1);
             }
             else
             {
                 anim.SetFloat("Direction", 0);
             }
         }*/

        // Vector3 movement = new Vector3(horizontal, vertical);
        /*
        if (turn > 0)
        {
            anim.SetFloat("Direction", 1);
        } else
        {
            anim.SetFloat("Direction", -1);

        }*/

        //transform.position += movement * Time.deltaTime;
        //Debug.Log(lookPos);
        //Debug.Log(Find("LookPos").GetComponent<FollowTarget>());
        //transform.rotation = lookPos.transform.rotation;

        //transform.rotation *= Quaternion.Slerp(
        //    Quaternion.identity, Quaternion.LookRotation(turn < 0 ? Vector3.left : Vector3.right), Mathf.Abs(turn) * turnSpeed * Time.deltaTime
        //    );
        float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
        anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す





        JumpWhenNeeded();

        if (isGrounded())
        {
            Debug.Log("isGrounded()");
            Debug.Log(isGrounded());

        }

        //if (direction != Vector3.zero)
        //{

        //    HandleRotation();
        //}

        //MoveInDirectionOfInput();

        //anim.SetFloat("Speed", vertical + horizontal);
    }

    private void PreventChildRotation()
    {
        lookPos.transform.rotation = Quaternion.Euler(0, -transform.rotation.y, 0);
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
                //anim.SetBool("Jump", true);
                //yield return new WaitForSeconds(1);
                //body.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
                //jumpStarts = Time.time;
            }
        }

        
    }

    //public void MoveInDirectionOfInput()
    //{
    //    Vector3 dir = Vector3.zero;

    //    dir.x = Input.GetAxis("Horizontal");
    //    dir.z = Input.GetAxis("Vertical");

    //    Vector3 camDirection = Camera.main.transform.rotation * dir; //This takes all 3 axes (good for something flying in 3d space)    
    //    Vector3 targetDirection = new Vector3(camDirection.x, 0, camDirection.z); //This line removes the "space ship" 3D flying effect. We take the cam direction but remove the y axis value

    //    if (dir != Vector3.zero)
    //    { //turn the character to face the direction of travel when there is input
    //        transform.rotation = Quaternion.Slerp(
    //        transform.rotation,
    //        Quaternion.LookRotation(targetDirection),
    //        Time.deltaTime * turnSpeed
    //        );
    //    }

    //    body.velocity = targetDirection.normalized * speed;     //normalized prevents char moving faster than it should with diagonal input

    //}

    //public void HandleRotation()
    //{
    //    float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
    //    Quaternion lookAt = Quaternion.Slerp(transform.rotation,
    //                                  Quaternion.Euler(0, targetRotation, 0),
    //                                  0.5f);
    //    transform.rotation = lookAt;
    //    //body.rotation = lookAt;

    //}

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
