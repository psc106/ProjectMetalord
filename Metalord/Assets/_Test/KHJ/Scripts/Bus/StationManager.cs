using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class StationManager : MonoBehaviour
{
    //정류장 프리팹 및 포지션 저장 리스트
    public List<GameObject> stationsPrefab = new List<GameObject>();
    public List <Vector3> savePositionList = new List<Vector3> ();
    public List <Button> stationBtns = new List<Button> ();

    public Canvas busUiCanvas = default;
    public GameObject buttonPrefab = default;


    private void Awake()
    {
        //followSpline = GetComponent<FollowSpline>();
        GetStationPosition();
        CreateBusUiContent();
    }
    public void GetStationPosition()
    {
        foreach (var station in stationsPrefab)
        {
            savePositionList.Add(station.gameObject.transform.position);
        }
        
    }
    public void OpenBusCanvas()
    {
        busUiCanvas.gameObject.SetActive(true);
    }
    public void CloseBusCanvas()
    {
        busUiCanvas.gameObject.SetActive(false);
    }
    private void CreateBusUiContent()
    {
        GameObject childObj = new GameObject("collectionOfButton");
        childObj.transform.parent = busUiCanvas.transform;
        
        //컴포넌트 추가
        childObj.AddComponent<RectTransform>();
        childObj.AddComponent<GridLayoutGroup>();

        //RectTransform 컴포넌트 변수에 추가 및 초기화
        RectTransform objRect = childObj.GetComponent<RectTransform>();
        objRect.anchoredPosition3D = Vector3.zero;
        objRect.localRotation = Quaternion.identity;
        objRect.localScale = Vector3.one;

        //GridLayGroup 컴포넌트 변수에 추가 및 초기화
        GridLayoutGroup objSort = objRect.GetComponent<GridLayoutGroup>();
        objSort.cellSize = new Vector2(150f, 100f);
        objSort.startAxis = GridLayoutGroup.Axis.Horizontal;
        objSort.childAlignment = TextAnchor.UpperLeft;
        objSort.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        objSort.constraintCount = 3;
        objRect.anchorMin.Set(0f, 0f);
        objRect.anchorMax.Set(1f, 1f);
        
        //버스 정류장 ui 추가
        for (int i = 0; i < stationsPrefab.Count; i++)
        {
            GameObject obj = Instantiate(buttonPrefab, childObj.transform);
            obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = string.Format("{0}", stationsPrefab[i].name);
            stationBtns.Add(obj.gameObject.GetComponent<Button>());
        }
    }
    
}
