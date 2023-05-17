using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMover : MonoBehaviour
{

    public float speed = 6;
    public float turnSpeed = 2;
    public Animator anim;
    private AnimatorStateInfo currentState;     // 現在のステート状態を保存する参照
    private AnimatorStateInfo previousState;    // ひとつ前のステート状態を保存する参照


    void Start()
    {
        // 各参照の初期化
        anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;




    }

// Update is called once per frame
void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);

        float turn = Input.GetAxis("Mouse X");





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

        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity, Quaternion.LookRotation(turn < 0 ? Vector3.left : Vector3.right), Mathf.Abs(turn) * turnSpeed * Time.deltaTime
            );
        float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
        anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す



        //anim.SetFloat("Speed", vertical + horizontal);
    }
}
