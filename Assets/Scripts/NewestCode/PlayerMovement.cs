using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IPlayerDeath
{
    [Header("Settings")]
    [SerializeField] float dashDuration = .25f;
    [SerializeField] float jumpDuration = .25f;
    [SerializeField] float jumpDistance = 100f;
    [SerializeField] float dashSpeed = 100f;
    [SerializeField] AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // Hidden from inspector
    public static float JumpCooldown { get; set; } = 1f;
    public static float DashCooldown { get; set; } = 1f;
    public static float JumpDistance { get; set; } = 100f;
    public static float DashSpeed { get; set; } = 100f;
    public static bool IsAlive { get; set; } = true;
    public static Action OnPlayerDeath { get; set; }

    public Action OnJumpStarted;
    public Action OnJumpEnded;
    public Action OnDashStarted;
    public Action OnDashEnded;

    private NavMeshAgent navMeshAgent;
    private Vector3 storedDestination;
    private Rigidbody2D rigidB;
    public bool isDashing = false;
    public bool isJumping = false;
    private bool canJump = true;
    private bool canDash = true;
    private float jumpTimer = 0f;
    private float dashTimer = 0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("Player NavMeshAgent is null on PlayerMovemenet");
        }
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        rigidB = GetComponent<Rigidbody2D>();
        if (rigidB == null)
        {
            Debug.LogError("Player Rigidbody2D is null on PlayerMovemenet");
        }
    }

    private void Start()
    {
        JumpDistance = jumpDistance;
        DashSpeed = dashSpeed;
        CursorManager.Instance.OnMouseLeftClick += OnLeftClick;
        CursorManager.Instance.OnMouseRightClick += OnRightClick;
    }

    private void Update()
    {
        if (!IsAlive)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpClick();
        }

        JumpStatus();
        DashStatus();
    }

    private void DashStatus()
    {
        if (dashTimer < DashCooldown)
        {
            dashTimer += Time.deltaTime;
            canJump = false;
            return;
        }
        if (isDashing)
        {
            canDash = false;
            return;
        }
        canDash = true;
    }

    private void JumpStatus()
    {
        if (jumpTimer < JumpCooldown)
        {
            jumpTimer += Time.deltaTime;
            canJump = false;
        }
        if (isJumping)
        {
            canJump = false;
            return;
        }
        if (isDashing)
        {
            canJump = false;
            return;
        }
        canJump = true;
    }

    private void OnLeftClick(Vector3 vector) // nav mesh movemment
    {
        if (!IsAlive) return;
        if (isDashing) return;
        if (!navMeshAgent.isActiveAndEnabled) return;
        Vector3 destination = GetSafeDestination(vector);
        navMeshAgent.SetDestination(destination);
    }

    private Vector3 GetSafeDestination(Vector3 destination)
    {
        NavMeshHit nearestPoint;
        if (NavMesh.SamplePosition(destination, out nearestPoint, 10f, NavMesh.AllAreas))
        {
           return nearestPoint.position;
        }
        else
        {
            return transform.position;
        }
    }

    public void PauseAgent()
    {
        if (navMeshAgent.pathPending == false)
        {
            storedDestination = navMeshAgent.destination;
        }
        navMeshAgent.isStopped = true;
    }

    public void ResumeAgent()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(GetSafeDestination(storedDestination));
    }

    private void OnRightClick(Vector3 vector) // dash movement
    {
        if (!canDash) return;
        isDashing = true;

        StartCoroutine(Dash());
        IEnumerator Dash()
        {
            OnDashStarted?.Invoke();
            PauseAgent();
            GetComponent<Collider2D>().isTrigger = true;
            var dashDirection = (vector - transform.position).normalized;
            var dashTime = 0f;
            do
            {
                if (!IsAlive) break;

                transform.position += (Vector3)(dashDirection * DashSpeed * Time.deltaTime);

                dashTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (dashTime < dashDuration);
            GetComponent<Collider2D>().isTrigger = false;
            OnDashEnded?.Invoke();
            ResumeAgent();
            dashTimer = 0f;
            isDashing = false;
        }
    }

    private void OnJumpClick()
    {
        if (!canJump) return;
        isJumping = true;

        StartCoroutine(Jump());
        IEnumerator Jump()
        {
            OnJumpStarted?.Invoke();
            var direction = rigidB.velocity.normalized;
            var jumpTime = 0f;
            Vector3 startingScale = transform.localScale;
            Vector3 zenithScale = transform.localScale * 1.5f;

            float t = 0f;
            do
            {
                if (!IsAlive) break;

                if (!isDashing)
                {
                    transform.position += (Vector3)(direction * JumpDistance * Time.deltaTime);
                }

                t = Mathf.Clamp01(jumpTime / jumpDuration);
                transform.localScale = Vector3.Lerp(startingScale, zenithScale, jumpCurve.Evaluate(t));
                jumpTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (jumpTime < jumpDuration);

            OnJumpEnded?.Invoke();
            transform.localScale = startingScale;
            isJumping = false;
            jumpTimer = 0f;
        }
    }
}
