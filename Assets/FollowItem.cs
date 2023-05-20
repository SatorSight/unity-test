using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowItem : MonoBehaviour
{
    public GameObject target;
    public float distanceFromObject = 1f;
    public float distanceFromPlayer = 5f;
    private Camera camera;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // text should be above the object its attached to
        Vector3 targetPos = target.transform.position;
        targetPos.y += distanceFromObject;
        transform.position = targetPos;

        // face towards camera but reverse rotation, otherwise text is displayed backwards
        transform.rotation = Quaternion.LookRotation((transform.position - camera.transform.position).normalized);

        if (isVisibleToPlayer())
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }

        processItemInteraction();
    }

    private void processItemInteraction()
    {
        if (isVisibleToPlayer())
        {

            if (Input.GetKeyDown("e") == true)
            {
                Debug.Log("E pressed");
                addToPlayerInventory();
                itemTaken();
            }
        }
    }

    private bool isVisibleToPlayer()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance < distanceFromPlayer && isInView();

    }

    private bool isInView()
    {
        Vector3 cameraForwardVec = camera.transform.forward;
        Vector3 toObjVec = transform.position - camera.transform.position;

        cameraForwardVec.Normalize();
        toObjVec.Normalize();

        float cameraAngle = Vector3.Dot(cameraForwardVec, toObjVec);

        // check if angle between camera forward vector and object->camera vector are facing almost same direction
        // feels around 20-30 degrees around center
        if (cameraAngle > 0.99f)
        {
            return true;
        }
        return false;
    }

    private void itemTaken()
    {
        Destroy(target);
        Object.Destroy(this.gameObject);
    }

    private void addToPlayerInventory()
    {
        GirlMover other = (GirlMover)player.GetComponent(typeof(GirlMover));

        other.addToInventory();
    }
}
