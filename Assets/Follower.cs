using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Follower : MonoBehaviour
{
    public FollowTarget target;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.3f);

    }
}
