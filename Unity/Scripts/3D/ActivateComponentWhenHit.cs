/*Copyright Jeremy Blair 2021
License (Creative Commons Zero, CC0)
http://creativecommons.org/publicdomain/zero/1.0/

You may use these scripts in personal and commercial projects.
Credit would be nice but is not mandatory.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class ActivateComponentWhenHit : MonoBehaviour
{
    public enum ActiveMode { enable,disable }
    public Component ComponentToActivateOnCollision;
    public ActiveMode mode = ActiveMode.enable;
    public bool AllowToggle = false;//allows the state to be reset if its already enabled/disabled.
    private FieldInfo fieldInfo;
    private PropertyInfo propertyInfo;
    public double SecondsToAllowNextActivation = 1;
    
    DateTime lastToggleTime;

    // Start is called before the first frame update
    void Start()
    {
        lastToggleTime = DateTime.Now.AddSeconds(-SecondsToAllowNextActivation);
        FieldInfo[] myFieldInfo;

        Type myType = ComponentToActivateOnCollision.GetType();
        // Get the type and fields of FieldInfoClass.
        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

        for (int i = 0; i < myFieldInfo.Length; i++)
        {
            if (myFieldInfo[i].Name == "enabled")
            {
                fieldInfo = myFieldInfo[i];
            }
        }

        if(fieldInfo==null)
        {
            PropertyInfo[] myPropertyInfo;
            myPropertyInfo = myType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance
           | BindingFlags.Public);

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                if (myPropertyInfo[i].Name == "enabled")
                {
                    propertyInfo = myPropertyInfo[i];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TagOfObjectToCauseActivation = "Player";


    private void OnTriggerEnter(Collider collision)
    {
        if (enabled && (DateTime.Now - lastToggleTime).TotalSeconds > SecondsToAllowNextActivation)
        {
            if (collision.gameObject.tag.ToLower() == TagOfObjectToCauseActivation.ToLower())
            {
                if (fieldInfo != null)
                {
                    bool currentValue = (bool)fieldInfo.GetValue(ComponentToActivateOnCollision);
                    if(mode == ActiveMode.enable)
                    {
                        if(!currentValue)
                            fieldInfo.SetValue(ComponentToActivateOnCollision, true);
                        else if(currentValue && AllowToggle)
                            fieldInfo.SetValue(ComponentToActivateOnCollision, false);
                    }
                    else {

                        if (currentValue)
                            fieldInfo.SetValue(ComponentToActivateOnCollision, false);
                        else if (!currentValue && AllowToggle)
                            fieldInfo.SetValue(ComponentToActivateOnCollision, true);
                    }
                    
                }
                if (propertyInfo != null)
                {
                    bool currentValue = (bool)propertyInfo.GetValue(ComponentToActivateOnCollision);
                    if (mode == ActiveMode.enable)
                    {
                        if (!currentValue)
                            propertyInfo.SetValue(ComponentToActivateOnCollision, true);
                        else if (currentValue && AllowToggle)
                            propertyInfo.SetValue(ComponentToActivateOnCollision, false);
                    }
                    else
                    {
                        if (currentValue)
                            propertyInfo.SetValue(ComponentToActivateOnCollision, false);
                        else if (!currentValue && AllowToggle)
                            propertyInfo.SetValue(ComponentToActivateOnCollision, true);
                    }

                }
            }
            lastToggleTime = DateTime.Now;
        }
    }
}

