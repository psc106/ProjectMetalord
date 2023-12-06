using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller_Physics : MonoBehaviour
{
    [SerializeField]
    LayerMask probeMask = -1;
    [SerializeField]
    LayerMask stairMask = -1;
    [SerializeField]
    LayerMask climbMask = -1;

    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform playerInputSpace = default;
    [SerializeField]
    TrailRenderer trailRenderer;


    Rigidbody connectedBody;
    Rigidbody previousConnectedBody;


    Vector3 input = Vector3.zero;
    Vector3 inputMouse = Vector3.zero;

    Vector3 velocity;
    Vector3 connectionVelocity;
    //Vector3 velocity = Vector3.zero;
    //Vector3 desireVelocity = Vector3.zero;
    //Vector3 connectionVelocity = Vector3.zero;

    Vector3 upAxis;
    Vector3 rightAxis;
    Vector3 forwardAxis;

    Vector3 connectionWorldPosition;
    Vector3 connectionLocalPosition;

    Vector3 contactNormal;
    Vector3 steepNormal;
    Vector3 climbNormal;
    Vector3 lastClimbNormal;

    float minGroundDotProduct;
    float minObjectDotProduct;
    float minClimbDotProduct;
    float currMouseSpeed = 0;

    bool desireClimb = true;
    bool desireJump = false;
    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;
    bool OnClimb => climbContactCount > 0;

    int jumpPhase = 0;
    int stepsSinceLastGrounded = 0;
    int stepsSinceLastJump = 0;


    int groundContactCount = 0;
    int steepContactCount = 0;
    int climbContactCount = 0;

    [SerializeField, Range(0, 100f)]
    float jumpHeight = default;
    [SerializeField, Min(0f)]
    float probeDistance = default;

    [SerializeField, Range(0, 100)]
    float maxMoveSpeed = default;
    [SerializeField, Range(0, 100)]
    float maxAirMoveSpeed = default;
    [SerializeField, Range(0,100f)]
    float maxClimbSpeed = default;

    [SerializeField, Range(0, 100)]
    float maxAcceleration = default;
    [SerializeField, Range(0, 100)]
    float maxAirAcceleration = default;
    [SerializeField, Range(0, 100)]
    float maxClimbAcceleration = default;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = default;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = default;

    [SerializeField, Range(0, 90f)]
    float maxGroundAngle = default;
    [SerializeField, Range(0, 90f)]
    float maxObjectAngle = default;
    [SerializeField, Range(90, 180f)]
    float maxClimbAngle = default;

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minObjectDotProduct = Mathf.Cos(maxObjectAngle * Mathf.Deg2Rad);
        minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                rb = GetComponentInChildren<Rigidbody>();
                if (!rb)
                {
                    rb = gameObject.AddComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
        rb.useGravity = false;
        OnValidate();
    }

    void Update()
    {
        //입력
        //Vector3 forward = playerInputSpace ? playerInputSpace.forward : Vector3.forward;
        //Vector3 right = playerInputSpace ? playerInputSpace.right : Vector3.right;
        //forward.y = 0;
        //right.y = 0;

        //input = Input.GetAxis("Horizontal") * right.normalized;
        //input += Input.GetAxis("Vertical") * forward.normalized;

        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        input.y = 0;

        //입력값 변환
        input = Vector3.ClampMagnitude(input, 1);

        if (playerInputSpace)
        {
            rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
        }
        else
        {
            rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
            forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
        }

        if (OnClimb)
        {
           // transform.rotation = Quaternion.Euler(climbNormal);
        }

        //desireVelocity = input * maxMoveSpeed;
        desireJump |= Input.GetButtonDown("Jump");

        //디버그
//        Debug.Log(OnGround + "/" + OnSteep + "/" + OnClimb);

        //Debug.Log(input);
        //Debug.Log(desireVelocity);
        Color color = new Color(0, 0, 0, 1);
        color.r = OnGround ? 1 : 0;
        color.g = OnSteep ? 1 : 0;
        color.b = OnClimb ? 1 : 0;

        GetComponent<Renderer>().material.color = color;
    }

    private void FixedUpdate()
    {
        //upAxis = -Physics.gravity.normalized;
        Vector3 gravity = CustomGravity.GetGravity(rb.position, out upAxis);
        //상태 업데이트
        UpdateState();

        //속도 계산
        AdjustVelocity();

        //점프
        if (desireJump)
        {
            desireJump = false;
            Jump(gravity);
        }
        if (OnClimb)
        {
            velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
        }
        else if(OnGround && velocity.sqrMagnitude < 0.01f)
        {
            velocity += contactNormal * (Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
        }
        else if(desireClimb && OnGround)
        {
            velocity += (gravity - contactNormal * maxClimbAcceleration * 0.9f) * Time.deltaTime;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
        //이동 
        rb.velocity = velocity;
        
        //상태 초기화
        ClearState();
    }

 

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }


    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void AdjustVelocity()
    {
        float acceleration;
        float speed;

        //기준이 되는 축을 해당 각도로 올린다.
        //Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        //Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        //Vector3 xAxis = ProjectDirectionOnPlane(rightAxis, contactNormal);
        //Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);
        Vector3 xAxis;
        Vector3 zAxis;
        if (OnClimb)
        {
            acceleration = maxClimbAcceleration;
            speed = maxClimbSpeed;
            xAxis = Vector3.Cross(contactNormal, upAxis);
            zAxis = upAxis;
        }
        else
        {
            acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
            speed = OnGround && desireClimb? maxClimbSpeed: maxMoveSpeed;
            xAxis = rightAxis;
            zAxis = forwardAxis;
        }
        xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
        zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

       // Debug.Log(xAxis + "/" + zAxis);

        Vector3 relativeVelocity = velocity - connectionVelocity;

        //이동 방향을 현재 기울기에 따라 힘 조절한다. 
        float currX = Vector3.Dot(relativeVelocity, xAxis);
        float currZ = Vector3.Dot(relativeVelocity, zAxis);

        //가속
        //float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        //원하는 속도로 가속/감속한다.
        float newX = Mathf.MoveTowards(currX, input.x * speed, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currZ, input.z * speed, maxSpeedChange);

        //새로운 속도와 현재 속도의 차이만큼 가속 시킨다.
        velocity += xAxis * (newX - currX) + zAxis * (newZ - currZ);
    }

    void Jump(Vector3 gravity)
    {
        Vector3 jumpDirection;
        if (OnClimb)
        {
            /*if(input.magnitude != 0)
            {
                Vector3 xAxis = Vector3.Cross(contactNormal, upAxis);
                Vector3 zAxis = upAxis;
                jumpDirection = xAxis* input.x + zAxis*input.z;
                jumpDirection.Normalize();
            }
            else
            {
                jumpDirection = contactNormal;
            }*/
            jumpDirection = contactNormal;
        }
        else if (OnGround)
        {
            //jumpDirection = contactNormal;
            jumpDirection = upAxis;
        }
        else if (OnSteep)
        {
            jumpDirection = steepNormal;
            //jumpDirection = upAxis;
            jumpPhase = 0;
        }
        else if (maxAirJumps > 0 &&jumpPhase <= maxAirJumps)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
            //jumpDirection = upAxis;
        }
        else
        {
            return;
        }

        jumpDirection = (jumpDirection + upAxis).normalized;


        stepsSinceLastJump = 0;
        jumpPhase += 1;

        //Root(-2*g*h) = 점프 속도
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);

        //float alignSpeed = Vector3.Dot(velocity, contactNormal);
        float alignSpeed = Vector3.Dot(velocity, jumpDirection);
        //계산전에 항상 이전 속도를 뺀다.
        //만약 이전 속도가 점프속도보다 빠를경우를 대비하여 0으로 빠르게 떨어질 경우 잠깐 멈추게 만든다.
        if (alignSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignSpeed, 0f);
        }
        //velocity.y += jumpSpeed;
        //velocity += contactNormal * jumpSpeed;
        velocity += jumpDirection * jumpSpeed;
        
    }


    void EvaluateCollision(Collision collision)
    {
        //여기서 현재 평면이 이동 가능한 절벽인지 체크?

        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        //모든 contact를 검사하여 일정 각도 이상의 평면을 모두 저장한다.
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);

            //onGround |= normal.y >= minGroundDotProduct;

            //cos에서 y값은 1->-1로 가므로 높을수록 각도는 낮은 각도
            if (upDot >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            }
            else
            {
                if (upDot > -0.01f)
                {
                    steepContactCount += 1;
                    steepNormal += normal;
                    if (groundContactCount == 0)
                    {
                        connectedBody = collision.rigidbody;
                    }
                }
                if(desireClimb && upDot >= minClimbDotProduct && (climbMask & (1<<layer)) != 0)
                {
                    climbContactCount += 1;
                    climbNormal += normal;
                    lastClimbNormal += normal;
                    connectedBody = collision.rigidbody;
                }
            }
            
        }
    }

    private void ClearState()
    {
        //매 프레임 마다 초기화한다.(FixedUpdate의 마지막)
        //행동(fixedUpdate)->초기화(fixedUpdate)->입력(update)->충돌처리(oncollision)->...
        //땅
        groundContactCount = 0;
        contactNormal = Vector3.zero;
        //가파른 경사
        steepContactCount = 0;
        steepNormal = Vector3.zero;
        //등산가능
        climbContactCount = 0;
        climbNormal = Vector3.zero;

        connectionVelocity = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
    }

    private void UpdateState()
    {
        //마지막 그라운드에서 몇 프레임이 지났는지 저장하기 위한 변수
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = rb.velocity;

        //확실히 땅이거나, 땅에 붙어있을 경우
        if (CheckClimbing() || OnGround || SnapToGround() || CheckSteepContacts())
        {
            //그라운드 변수 초기화
            //점프 횟수 초기화
            //만약 땅에 1개 이상 밟고 잇을 경우 단위 벡터로 초기화
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
            {
                jumpPhase = 0;
            }
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }

        //공중에 있을 경우
        else
        {
            //접촉 방향 Vector3.up(평지)
            contactNormal = upAxis;
        }

        if (connectedBody)
        {
            if (connectedBody.isKinematic || connectedBody.mass >= rb.mass)
            {
                UpdateConnectionState();
            }
        }

    }

    void UpdateConnectionState()
    {
        if (connectedBody == previousConnectedBody)
        {
            Vector3 connectionMovement = connectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        connectionWorldPosition = rb.position;
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);

    }
   
    bool SnapToGround()
    {
        //만약 땅에서 벗어나고 2프레임 이상 진행 됐을경우
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false;
        }
        float speed = velocity.magnitude;
        //만약 현재 속도가 일정 속도 이상이라면
        if (speed > maxSnapSpeed)
        {
            return false;
        }

        //땅으로 레이를 쐈을 때 hit되지않을 경우
        if (!Physics.Raycast(rb.position, -upAxis, out RaycastHit hit, probeDistance, probeMask))
        {
            return false;
        }

        float upDot = Vector3.Dot(upAxis, hit.normal);
        //hit한 곳이 유효하지 않은 경사일경우(maxSlopeAngle을 넘길경우)
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        //1프레임까지는 붙어있다고 판단.
        groundContactCount = 1;
        contactNormal = hit.normal;

        float dot = Vector3.Dot(velocity, hit.normal);

        //만약 velocity가 바닥을 향하고 있다면 더 느려지는 경우가 있기 때문에 이 경우를 제외한다.
        if (dot > 0f)
        {
            //velocity를 사영하여 각도를 바꾸고 magnitude를 곱해서 동일한 힘을 준다.
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }

        connectedBody = hit.rigidbody;
        return true;
    }


    bool CheckSteepContacts()
    {
        //만약 가파른 경사를 2개 이상 얻고있다면
        //가상의 경사를 만든다.
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }

    bool CheckClimbing()
    {
        if (OnClimb)
        {
            if(climbContactCount > 1)
            {
                climbNormal.Normalize();
                float upDot = Vector3.Dot(upAxis, climbNormal);
                if(upDot >= minGroundDotProduct)
                {
                    climbNormal = lastClimbNormal;
                }
            }
            groundContactCount = 1;
            contactNormal = climbNormal;
            return true;
        }
        return false;
    }


    float GetMinDot(int layer)
    {
        return (stairMask & (1 << layer)) == 0 ? minGroundDotProduct : minObjectDotProduct;
    }
    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        //vector를 normal의 각도 만큼 Projection 한다.
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }
    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        //vector를 normal의 각도 만큼 Projection 한다.
        return (direction - normal*Vector3.Dot(direction, normal)).normalized;
    }

}
