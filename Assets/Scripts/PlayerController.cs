using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float applyForce;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Aplicar a força no objeto
        rb.AddForce(new Vector2(
            -Input.GetAxis("Horizontal") * applyForce,
            -Input.GetAxis("Vertical") * applyForce
        ), ForceMode2D.Force);

    }

    void FixedUpdate()
    {

    }
}
