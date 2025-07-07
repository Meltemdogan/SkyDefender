using System.Collections;
using _Game.Scripts.Enemies.Interfaces;
using _Game.Scripts.Projectiles;
using ToolBox.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Enemies
{
    public class EnemyDodger : BaseEnemy
    {
        [Header("Shooter Dodger Settings")]
        [SerializeField] private float detectionRange = 6f;
        [SerializeField] private float fireCooldown   = 1.5f;
        [SerializeField] private float dodgeSpeed     = 5f;
        [SerializeField] private float dodgeDuration  = 1f;
        [SerializeField] private float dodgeCooldown  = 3f;
        [SerializeField] private float wanderTime     = 4f;

        private bool  isWandering = false;
        private bool  isDodging   = false;
        private bool  canDodge    = true;
        private float fireTimer   = 0f;
        private Vector2 movementDirection;

        protected override void Start()
        {
            base.Start();
            // face the city initially
            SetDirectionToCity();
        }

        protected override void MoveBehavior()
        {
            // if already wandering or dodging, skip decision tree
            if (isWandering || isDodging) 
                return;

            if (player == null)
            {
                Patrol();
                return;
            }

            float distance     = Vector2.Distance(transform.position, player.position);
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            float dot          = Vector2.Dot(transform.up, dirToPlayer);

            if (distance > detectionRange)
            {
                Patrol();
            }
            else if (dot < -0.5f && canDodge)
            {
                StartCoroutine(DodgePlayer(dirToPlayer));
            }
            else if (dot > 0.5f)
            {
                // chase & shoot
                fireTimer += Time.deltaTime;
                if (fireTimer >= fireCooldown)
                {
                    Shoot();
                    fireTimer = 0f;
                }

                SmoothRotateTowards(dirToPlayer);
                transform.position += (Vector3)transform.up * moveSpeed * Time.deltaTime;
            }
            else
            {
                // neither clearly in front nor behind → wander briefly
                StartCoroutine(WanderThenReturnToCity());
            }
        }

        private void Patrol()
        {
            SmoothRotateTowards(movementDirection);
            transform.position += (Vector3)movementDirection * moveSpeed * Time.deltaTime;
        }

        private IEnumerator DodgePlayer(Vector2 dirToPlayer)
        {
            isDodging = true;
            canDodge  = false;

            Vector2 dodgeDir = Vector2.Perpendicular(dirToPlayer).normalized * (Random.value > 0.5f ? 1 : -1);
            Vector2 backDir  = -dirToPlayer;
            float   timer    = 0f;

            while (timer < dodgeDuration)
            {
                timer += Time.deltaTime;
                // combine backward and sideways
                Vector2 moveDir = (backDir + dodgeDir).normalized;
                SmoothRotateTowards(moveDir);
                transform.position += (Vector3)moveDir * dodgeSpeed * Time.deltaTime;
                yield return null;
            }

            isDodging = false;
            yield return new WaitForSeconds(dodgeCooldown);
            canDodge  = true;
        }

        private IEnumerator WanderThenReturnToCity()
        {
            isWandering = true;

            // pick a random 2D direction
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float  timer     = 0f;

            while (timer < wanderTime)
            {
                timer += Time.deltaTime;
                SmoothRotateTowards(randomDir);
                transform.position += (Vector3)randomDir * moveSpeed * Time.deltaTime;
                yield return null;
            }

            // after wandering, redirect toward city
            SetDirectionToCity();
            isWandering = false;
        }

        private void SetDirectionToCity()
        {
            movementDirection = (CityController.Instance.transform.position - transform.position).normalized;
            SmoothRotateTowards(movementDirection);
        }

        protected override void Shoot()
        {
            if (!canFire) return;

            Debug.Log($"{gameObject.name} is shooting!");
            var bulletObj = projectilePrefab.Reuse();
            bulletObj.transform.position = firePoint.transform.position;
            bulletObj.transform.rotation = firePoint.transform.rotation;
            bulletObj.GetComponent<IProjectile>().Initialize(projectileData);
        }

        public override void TakeDamage(int amount)
        {
            if (!healthSystem.IsAlive) return;

            healthSystem.DecreaseCurrentHealthBy(amount);
            if (!healthSystem.IsAlive)
                Die();
        }
    }
}
