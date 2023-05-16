using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlMover : MonoBehaviour
{

    public float speed = 6;
    public float turnSpeed = 2;
    public Animator anim;


    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);

        float turn = Input.GetAxis("Mouse X");


        // Vector3 movement = new Vector3(horizontal, vertical);


        //transform.position += movement * Time.deltaTime;

        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity, Quaternion.LookRotation(turn < 0 ? Vector3.left : Vector3.right), Mathf.Abs(turn) * turnSpeed * Time.deltaTime
            );

        anim.SetFloat("Speed", vertical);
    }
}
