using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody _RigidBody;
    float MovementSpeed = 3.0f;
    float JumpForce = 1.5f;
    public float DestinationToGround = 0.1f;
    public int Strength = 10;
    public GameObject AttackPoint;
    public float AttackRange = 0.7f;
    public LayerMask AttackedLayer;
    public int AttacCountdownSeconds = 1;
    private bool EnableAttack = true;
    bool IsGrounded = true;
    private AnimationManager _AnimationManager;
    void Start()
    {
        _RigidBody = GetComponent<Rigidbody>();
        _AnimationManager = GetComponent<AnimationManager>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        GroundCheck();
        if (Input.GetKey(KeyCode.Space) && IsGrounded) Jump();
        if (Input.GetKey(KeyCode.Mouse0) && EnableAttack) Attack();
        _RigidBody.MovePosition(CalculateMovement());
        SetRotation();

        SetPlayerAnimation();
    }
    Vector3 CalculateMovement()
    {
        float HorizontalDirection = Input.GetAxis("Horizontal");
        float VerticalDirection = Input.GetAxis("Vertical");
        Debug.Log(HorizontalDirection + " " + VerticalDirection);
        return _RigidBody.transform.position + new Vector3(HorizontalDirection, 0, VerticalDirection) * Time.fixedDeltaTime * MovementSpeed;
    }

    private void Jump()
    {
        _RigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
    private void GroundCheck()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, DestinationToGround);
    }
    private void SetRotation()
    {
        Plane PlayerPlane = new Plane(Vector3.up, transform.position);
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0;
        if (PlayerPlane.Raycast(Ray, out hitdist))
        {
            Vector3 TargetPoint = Ray.GetPoint(hitdist);
            Quaternion TargetRotation = Quaternion.LookRotation(TargetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, MovementSpeed * Time.deltaTime);
        }
    }
    
    private void SetPlayerAnimation()
    {
        if (IsGrounded)
        {
            if (Input.GetKey(KeyCode.Mouse0)) _AnimationManager.SetAnimationRun();
            else
            {
                if (IsMoving()) _AnimationManager.SetAnimationAttack();
                else _AnimationManager.SetAnimationIdle();
            }
        }
        else _AnimationManager.SetAnimationJump();
    }
    private void Attack()
    {
        Collider[] HitedColliders = Physics.OverlapSphere(AttackPoint.transform.position, AttackRange, AttackedLayer);
        foreach(Collider HitedCollider in HitedColliders)
        {
            IAttackable Attackable = HitedCollider.gameObject.GetComponent<IAttackable>();
            Attackable.DealDamage(Strength);
            Debug.Log(HitedCollider.name + " нанесено " + Strength + " урона ");
        }
        EnableAttack = false;
        StartCoroutine(AttackCountdown());
        
    }
    private IEnumerator AttackCountdown()
    {
        int Counter = AttacCountdownSeconds;
        while(Counter>0)
        {
            yield return new WaitForSeconds(1);
            Counter--;
        }
        EnableAttack = true;
    }

    private bool IsMoving()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) != Vector3.zero;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * DestinationToGround));
        Gizmos.DrawSphere(AttackPoint.transform.position, AttackRange);
    }
}
