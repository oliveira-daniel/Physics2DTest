using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float applyForce;
    private bool isInWater;
    public float UpwardForce = 9.81f;
    private bool downControl, upControl;
    public float downForce;
    private float actualForce;
    [SerializeField]
    private ParticleSystem downThrust, upThrust;
    private ParticleSystem.EmissionModule downEmission, upEmission;

    void Awake()
    {
        downEmission = downThrust.emission;
        upEmission = upThrust.emission;

        downEmission.enabled = false;
        upEmission.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInWater)
        {
            Vector2 force = transform.up * UpwardForce;
            rb.AddRelativeForce(force, ForceMode2D.Force);
        }

        downEmission.enabled = downControl;
        upEmission.enabled = upControl;

        if (upControl)
        {
            rb.AddRelativeForce(-transform.up * actualForce);
            upEmission.enabled = true;
        }

        if (downControl)
        {
            rb.AddRelativeForce(transform.up * actualForce);
            downEmission.enabled = true;
        }
    }

    void Update()
    {
        if (transform.position.y <= 0f)
        {
            isInWater = true;
            rb.drag = 5f;
            actualForce = downForce;
        }
        else
        {
            isInWater = false;
            rb.drag = 0.05f;
            actualForce = downForce * 0.9f;
        }

        // Controle do Jogador
        upControl = Input.GetKey(KeyCode.UpArrow);
        downControl = Input.GetKey(KeyCode.DownArrow);
    }
}
