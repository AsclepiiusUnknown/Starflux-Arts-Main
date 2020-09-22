using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float smooth;
    public float speed;

    private float z;
    private float x;
    private Vector3 targetPosition;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            speed = 1;
            smooth = 20;

            RaycastHit hit;

            //var playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //double hitdist = 0.0;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;

                x = hit.point.x;
                z = hit.point.z;
                //var targetRotation = Quaternion.LookRotation(targetPoint    - transform.position);
                //transform.rotation = targetRotation;
            }
        }


        StartCoroutine(Turn(targetPosition));

        //transform.position = Vector3.RotateTowards(transform.position, targetPosition, speed*Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * smooth);
    }

    IEnumerator Move(Vector3 _targetPosition)
    {
        // we want to rotate first
        yield return Turn(_targetPosition);
        // then translate
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * smooth);
    }

    IEnumerator Turn(Vector3 _targetPosition)
    {
        Quaternion srcRotation = transform.rotation;
        Quaternion dstRotation = Quaternion.LookRotation(_targetPosition);
        float percent = 0.1f;
        while (percent <= 1.0f)
        {
            Quaternion newRotation = Quaternion.Lerp(srcRotation, dstRotation, percent);
            transform.rotation = newRotation;
            percent += .01f;
            yield return null;
        }
    }


}
