using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour {

    public Image back, front;
    public int widht, height;
    public Text label;
    int _value;
    public int maxValue;
    static MySlider instance;
    static UnitElement CurUnit;
    public int value
    {
        set
        {
            _value = Mathf.Clamp(value, 0, maxValue);
            UpdateFront();
        }
    }

	void Start () {
        back.GetComponent<RectTransform>().sizeDelta = new Vector2(widht, height);
        back.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        front.GetComponent<RectTransform>().sizeDelta = new Vector2(widht, height);
        front.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
        instance = this;
    }

    void UpdateFront()
    {
        float valueWidht = (float)_value / maxValue * widht;
        front.GetComponent<RectTransform>().sizeDelta = new Vector2(valueWidht, height);
        front.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(widht - valueWidht) * 0.5f, 0);
        //label.text = _value + " / " + maxValue;
    }
    public static void SetUnit(UnitElement unit)
    {
        if (CurUnit == unit)
            unit = null;
        instance.gameObject.SetActive(unit);
        if(unit)
        {
            instance.maxValue = unit.MaxHP;
            instance.value = unit.HP;
            instance.label.text = unit.HP + " / " + unit.MaxHP + " (" + unit.AP + ")";
        }
        CurUnit = unit;
    }

}
