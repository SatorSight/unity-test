using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public float turnSpeed = 2;
    public float viewRange = 60.0f;
    public Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        float turn = Input.GetAxis("Mouse X");
        float turnV = Input.GetAxis("Mouse Y");

        Vector3 vecH = turn < 0 ? Vector3.left : Vector3.right;
        Vector3 vecV = turnV < 0 ? Vector3.down : Vector3.up;

        Quaternion horizontalRotation = Quaternion.LookRotation(vecH);
        Quaternion verticalRotation = Quaternion.LookRotation(vecV);

        horizontalRotation.eulerAngles = new Vector3(horizontalRotation.eulerAngles.x, horizontalRotation.eulerAngles.y, 0f);
        verticalRotation.eulerAngles = new Vector3(verticalRotation.eulerAngles.x, verticalRotation.eulerAngles.y, 0f);

        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity,
            horizontalRotation,
            Mathf.Abs(turn) * turnSpeed * Time.deltaTime
        );

        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity,
            verticalRotation,
            Mathf.Abs(turnV) * turnSpeed * Time.deltaTime
        );

        //Quaternion res = transform.rotation;

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        //float clampedX = Mathf.Clamp(eulerRotation.x, 20f, 340f);

        //Debug.Log("eulerRotation.x");
        //Debug.Log(eulerRotation.x);

        // 300 - 50
        // 0 - 90, 270 - 360
        float val = eulerRotation.x;

        if (val > 0 && val < 90)
        {
            //if (val > 70)
            //{
            //    val = 70;
            //}
            val = Mathf.Clamp(val, 0, 70);
        } else if (val > 200)
        // 270 - 360 range
        {
            //if (val < 270)
            //{
            //    val = 270;
            //}
            val = Mathf.Clamp(val, 290, 360);
        }


        transform.rotation = Quaternion.Euler(val, eulerRotation.y, 0);

        //camera.transform.localEulerAngles = new Vector3(Mathf.Clamp(camera.transform.localEulerAngles.x, -viewRange, viewRange), 0, 0);
    }
}
