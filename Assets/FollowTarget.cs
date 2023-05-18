using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public float turnSpeed = 2;


    // Update is called once per frame
    void Update()
    {


        float turn = Input.GetAxis("Mouse X");

        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity,
            Quaternion.LookRotation(turn < 0 ? Vector3.left : Vector3.right),
            Mathf.Abs(turn) * turnSpeed * Time.deltaTime
        );

    }
}
