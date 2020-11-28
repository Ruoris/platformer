using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;
        Rigidbody2D rb;
        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                {
                    control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
                }
            }
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("tÖRMÄTTY" + collision.gameObject.name);

            if (collision.gameObject.name == "Player")
            {
                Debug.Log("osuttu pelaajaan");
                var player = collision.gameObject.GetComponent<PlayerController>();
                player.CollidedWithEnemy(this);

            }
        }
        public void EnemyDeath()
        {
            _collider.enabled = false;
            // control.animator.SetTrigger("death");
            control.enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            if (_audio && ouch)
            {
                _audio.PlayOneShot(ouch);
            }


        }
    }
}