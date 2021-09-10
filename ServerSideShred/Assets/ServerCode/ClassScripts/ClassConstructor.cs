using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassConstructor : MonoBehaviour
{
    protected bool canAuto = true; 
    protected bool canQ = true;
    protected bool canC = true;
    protected bool canE = true; 
    protected bool canR = true;

    protected float setAuto { get; set; }
    protected float setQ { get; set; }
    protected float setC { get; set; }
    protected float setE { get; set; }
    protected float setR { get; set; }

    protected float timerAuto = 0f;
    protected float timerQ = 0f;
    protected float timerC = 0f;
    protected float timerE = 0f; 
    protected float timerR = 0f;

    public virtual void SetTimers()
    {
        timerAuto = setAuto;
        timerQ = setQ;
        timerC = setC;
        timerE = setE; 
        timerR = setR;
    }

    public virtual void AutoAbility()
    {

    }

    // Start is called before the first frame update
    public virtual  void QAbility()
    {

    }

    public virtual void CAbility()
    {

    }

    public virtual void EAbility()
    {

    }

    public virtual  void UltAbility()
    {

    }

    public virtual  void InteruptA()
    {

    }

    public virtual  void InteruptB()
    {

    }

    public virtual void InteruptC()
    {

    }

    public virtual void InteruptD()
    {

    }

    public virtual void InteruptE()
    {

    }
    public virtual void Final()
    {

    }
    //w, a, s, d,space in another function
    /// <summary>
    /// <param name="_inputs"> left click, q, c, e, r   </param>
    /// </summary>
    protected void ApplyInput(bool[] _inputs)
    {
        InteruptA();
        if (_inputs[0] && canAuto)
        {
            AutoAbility();
            //canAuto = false; 
        }
        InteruptB();
        if (_inputs[1] && canQ)
        {
            QAbility();
            //canQ = false;
        }
        InteruptC();
        if (_inputs[2] && canC)
        {
            CAbility();
            //canC = false;
        }
        InteruptD();
        if (_inputs[3] && canE)
        {
            EAbility();
            //canE = false;
        }
        InteruptE();
        if (_inputs[4] && canR)
        {
            UltAbility();
            //canR = false; 
        }
        Final();
    }

    protected void MainTimerCount()
    {
        if (!canAuto)
        {
            timerAuto -= Time.fixedDeltaTime;
            if (timerAuto <= 0)
            {
                timerAuto = setAuto;
                canAuto = true;

            }
        }
        if (!canQ)
        {
            timerQ -= Time.fixedDeltaTime;
            if(timerQ <= 0)
            {
                timerQ = setQ;
                canQ = true; 
            }
        }
        if(!canC)
        {
            timerC -= Time.fixedDeltaTime; 
            if(timerC <= 0)
            {
                timerC = setC; 
                canC = true; 
            }
        }
        if(!canE)
        {
            timerE -= Time.fixedDeltaTime; 
            if(timerE <= 0)
            {
                timerE = setE;
                canE = true; 
            }
        }
        if(!canR)
        {
            timerR -= Time.fixedDeltaTime;
            if (timerR <= 0)
            {
                timerR = setR;
                canR = true;
            }
        }
        
        
    }
    //i think make another dict and add to that with the updated 
    //protected void OtherTimerCount(Dictionary<int, float> _timers)
    //{
    //    foreach (KeyValuePair<bool, float> _timersInstance in _timers)
    //    {
    //        if (_timersInstance.Value > 0f) _timersInstance.Value -= Time.fixedTime;
    //    }
    //    return _timers;
    //}

    protected virtual void RenderProjectile(object Object)
    {

    }
}

    

