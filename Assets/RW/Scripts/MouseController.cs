using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    public float jetpackForce = 75.0f;

    private Rigidbody2D playerRigidbody;

    public float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;

    public ParticleSystem jetpack;

    private bool isDead = false;

    private uint coins = 0;

    public Text coinsCollectedLabel;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        mouseAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        if (!isDead)
        {
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = forwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;
        }
        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        //1
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        //2
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 75.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else
        {
            HitByLaser(collider);
        }
        //mouseAnimator.SetBool("isDead", true);
    }

    void HitByLaser(Collider2D laserCollider)
    {
        isDead = true;
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        Destroy(coinCollider.gameObject);
        coinsCollectedLabel.text = coins.ToString();
    }
}
