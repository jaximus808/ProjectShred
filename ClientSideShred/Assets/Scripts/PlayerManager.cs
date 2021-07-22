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
    public Text usernameDisplay;

    public int id;
    public string username;
    public string className;

    public Text[] timers;

    public Image[] icons;

    private bool[] active = new bool[5] {false, false, false, false, false}; 
    
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

    public void UpdateTimers(float _auto,float _Q,float _C,float _E,float _R)
    {
        float[] ablityTimer = new float[5]{_auto, _Q,_C,_E,_R};
        for(int i = 0; i < 5;i++)
        {
            if(ablityTimer[i] <= 0f)
            {
                timers[i].text = "";
                if(active[i])
                {
                    active[i] = false; 
                    icons[i].color = new Color32(255,255,255, 255);
                }
            } 
            else
            {
                timers[i].text = ablityTimer[i].ToString("F2");
                if(!active[i])
                {
                    active[i] = true; 
                    icons[i].color = new Color32(255,255,255, 20);
                }
            } 
        }
    }

    
}