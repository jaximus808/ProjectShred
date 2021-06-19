using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Transform cam;
    public BillBoard bill; 

    public int id;
    public string username;
    public string className; 

    public void SetMaxHeath(int _health)
    {
        Debug.Log(_health);
        slider.maxValue = _health;
        slider.value = _health;

    }

    public void UpdateHealth(int _health)
    {
        slider.value = _health;

    }
}