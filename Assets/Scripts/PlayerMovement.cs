using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;

    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        

        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

        transform.Translate(vertical, 0, horizontal);
    }
}
