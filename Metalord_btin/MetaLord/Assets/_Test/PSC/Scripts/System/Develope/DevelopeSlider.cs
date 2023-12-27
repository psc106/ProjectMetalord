
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DevelopeSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    Controller_Physics player;

    [SerializeField]
    Slider slider;
    [SerializeField]
    Text valueText;

    [SerializeField]
    SliderType type;

    private void Awake()
    {
        if(slider==null)
            slider = GetComponent<Slider>();
        if (valueText == null)
            valueText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        InitValue();
    }

    public void SlideValue()
    {
        valueText.text = slider.value.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetValue();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
    void InitValue()
    {
        if (player == null)
            player = FindObjectOfType<Controller_Physics>();
        slider.value = player.GetValue(type);
        valueText.text = slider.value.ToString();

    }

    void SetValue()
    {
        //slider.value = (slider.value);
        player.SetValue(type, slider.value);
        valueText.text = slider.value.ToString();
    }
}

public enum SliderType
{
    Move,
    Jump,
    Gravity,
    OneShot,
    repeatShot,
    Grab,
    Range,
    Capacity
}