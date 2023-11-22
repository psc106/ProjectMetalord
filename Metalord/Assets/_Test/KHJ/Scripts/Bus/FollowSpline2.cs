using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FollowSpline2 : MonoBehaviour
{
    //이동하게될 스플라인 컨테이너
    public SplineContainer mySpline;

    //오브젝트의 이동속도
    [SerializeField] private float objSpeed = 1f;
    //이동 거리
    float distancePercentage = 0f;
    float splineLength;

    //오브젝트 움직임 제어
    public bool isStop = default;
    public bool isOneToZero = false;
    int currentPosIdx = 0;
    public bool isArrived = false;
    //멈춰야 하는 목적지 포지션
    public Vector3 stopStationPos = Vector3.zero;

    //정류장 정보를 가져오기 위한 스크립트
    public StationManager stationInfo = default;

    //처음 정류장 도착 확인 위한 bool값
    public bool isStart;


    void Start()
    {
        AddOnButtonEvent();
        splineLength = mySpline.CalculateLength();
        isStart = false;
        isOneToZero = false;
        isStop = true;
        //currentPosIdx = stationInfo.stationBtns.Count;
    }
    
    void Update()
    {
        if (!isStop)
        {
            //FollowZeroToOne();

            if (!isOneToZero)
            {
                FollowZeroToOne();
            }
            else
            {
                FollowOneToZero();
            }
        }
        if(Input.GetKeyDown(KeyCode.K)) 
        {
            stationInfo.OpenBusCanvas();
        }
    }
    //{정방향 역방향 bool값 온 오프
    public void FollowZeroToOne()
    {
        distancePercentage += objSpeed * Time.deltaTime / splineLength;
        Vector3 currentPosition = mySpline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;

            //return;
        }
        
        Vector3 nextPosition = mySpline.EvaluatePosition(distancePercentage + 0.00001f);
        Vector3 direction = nextPosition - currentPosition;
        Vector3 perpendicularVector = Vector3.Cross(direction, transform.up);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, perpendicularVector.y, direction.z), transform.up);
    }
    public void FollowOneToZero()
    {
        distancePercentage -= objSpeed * Time.deltaTime / splineLength;
        Vector3 currentPosition = mySpline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage < 0f)
        {
            distancePercentage = 1f;
            //return;
        }

        Vector3 nextPosition = mySpline.EvaluatePosition(distancePercentage - 0.00001f);
        Vector3 direction = nextPosition - currentPosition;
        Vector3 perpendicularVector = Vector3.Cross(direction, transform.up);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, perpendicularVector.y, direction.z), transform.up);
    }
    public void StopBus()
    {
        isStop = true;
    }
    public void GoBus()
    {
        isStop = false;
    }
    //}정방향 역방향 bool값 온 오프

    //{버튼에 이벤트 추가
    private void AddOnButtonEvent()
    {
        for (int i = 0; i < stationInfo.stationBtns.Count; i++)
        {
            int currentIndex = i;
            stationInfo.stationBtns[i].onClick.AddListener(() => GoBus());
           
            stationInfo.stationBtns[i].onClick.AddListener(() => GoStation(currentIndex));
        
        }
    }
    //}버튼에 이벤트 추가

    //{온트리거 이벤트
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BusStop")
        {
            CheckStopPoint(stopStationPos, other.transform.position);
        }
        if (other.tag == "LoadableItem")
        {
            AddLoadableItem();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BusStop")
        {
            isArrived = false;
            stationInfo.CloseBusCanvas();
        }
    }
    //}온트리거 이벤트

    //{멈추는 조건 확인
    private void CheckStopPoint(Vector3 goalPos, Vector3 checkPos)
    {
        if (goalPos == checkPos)
        {
            StopBus();
            stationInfo.OpenBusCanvas();
            isArrived = true;
        }
        else
        {
            isArrived = false;
            stationInfo.CloseBusCanvas();
            GoBus();
        }
    }
    //}멈추는 조건 확인

    //{플레이어 조작 출발
    public void GoStation(int goalPositionIdx)
    {
        isArrived = false;

        stopStationPos = stationInfo.savePositionList[goalPositionIdx];

        if(mySpline.Spline.Closed == false)
        {
            Debug.Log(mySpline.Spline.Closed);
            if (currentPosIdx > goalPositionIdx)
            {
                isOneToZero = true;
            }
            else
            {
                stationInfo.CloseBusCanvas();
                isOneToZero = false;
            }
            if (currentPosIdx == goalPositionIdx)
            {
                //이거때문에 못움직이는 거였다.처음에
                isStop = true;
                return;
            }
        }
        else if(mySpline.Spline.Closed == true)
        {
            Debug.Log(mySpline.Spline.Closed);

            CheckDirection(currentPosIdx, goalPositionIdx);
        }
        currentPosIdx = goalPositionIdx;
    }
    //} 플레이어 조작 출발

    private void CheckDirection(int nowIdx, int destinationIdx)
    {
        //CheckDirection(currentPosIdx, goalPositionIdx); 사용예
        
        int forwardD;
        int reverseD;
        Debug.LogFormat("현재{0}",nowIdx);
        Debug.LogFormat("목적지{0}",destinationIdx);
        if (nowIdx > destinationIdx)
        {
            forwardD = nowIdx - destinationIdx; //6
            reverseD = (stationInfo.stationBtns.Count - nowIdx) +  destinationIdx; //2
                Debug.Log("현재 위치 인덱스가 목표 위치 인덱스보다 클 때");
            if (forwardD > reverseD)
            {
                isOneToZero = false;
            }
            else if (nowIdx == destinationIdx)
            {
                //이거때문에 못움직이는 거였다.처음에
                isStop = true;
                if (nowIdx == 0 && isStart == false)
                {
                    isStart = true;
                    isStop = false;
                }
                return;
            }
            else
            {
                isOneToZero = true;
            }
        }
        else
        {
            forwardD = destinationIdx - nowIdx; //6
            reverseD = (stationInfo.stationBtns.Count - destinationIdx) + nowIdx;
            Debug.Log("목표 위치 인덱스가 현재 위치 인덱스보다 클 때");
            if (forwardD > reverseD)
            {
                isOneToZero = true;
            }
            else if (nowIdx == destinationIdx)
            {
                //이거때문에 못움직이는 거였다.처음에
                isStop = true;
                if (nowIdx == 0 && isStart == false)
                {
                    isStart = true;
                    isStop = false;
                }
                return;
            }
            else
            {
                isOneToZero = false;
            }
        }
        

    }

    //나중에 아이템 추가
    public virtual void AddLoadableItem()
    {
        //여기에 추가?
    }

}
