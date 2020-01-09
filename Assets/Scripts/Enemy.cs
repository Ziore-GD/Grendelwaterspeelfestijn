using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), typeof (BoxCollider))]

public class Enemy : MonoBehaviour {
    private float health = 100;
    [SerializeField] private bool dead = false;
    [SerializeField] private ParticleSystem _onHitFX;
    public float speed = 2f;
    Rigidbody rb;
    BoxCollider col;
    SpriteRenderer[] renderers;

    public bool IsDead {
        get {
            return dead;
        }
    }

    public void SetHealth (float h) {
        health = h;
    }

    void Awake () {
        rb = GetComponent<Rigidbody> ();
        col = GetComponent<BoxCollider> ();
        renderers = GetComponentsInChildren<SpriteRenderer> ();
    }

    void Update () {
        Walk ();
    }

    public void DeltaHealth (float delta) {
        health -= delta;
        _onHitFX.Emit (Random.Range (5, 10));
        foreach (SpriteRenderer renderer in renderers) {
            renderer.color = new Color (255, 255, 255, 255 * (health/ 100));
        }

        if (health <= 0) {
            Die ();
        }
    }

    private void Die () {
        dead = true;
        transform.rotation = Quaternion.Euler (0, 180, -90);
        enabled = false;
        col.enabled = false;
        rb.velocity = Vector3.zero;
        Invoke ("Hide", 2);
    }

    private void Hide () {
        gameObject.SetActive (false);
    }
    public void Show () {
        dead = false;
        transform.rotation = Quaternion.Euler (0, 180, 0);
        col.enabled = true;
        CancelInvoke ();
        gameObject.SetActive (true);
    }

    private void Walk () {
        rb.velocity = new Vector3 (1f * speed * Time.deltaTime, 0, 0);
    }
}