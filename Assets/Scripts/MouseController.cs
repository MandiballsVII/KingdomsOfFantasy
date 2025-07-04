using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{
    public Action<RaycastHit> OnLeftMouseClick;
    public Action<RaycastHit> OnRightMouseClick;
    public Action<RaycastHit> OnMiddleMouseClick;
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // Left mouse button
        {
            CheckMouseClick(0);
        }
        if(Input.GetMouseButtonDown(1)) // Right mouse button
        {
            CheckMouseClick(1);
        }
        if(Input.GetMouseButtonDown(2)) // Middle mouse button
        {
            CheckMouseClick(2);
        }
    }

    private void CheckMouseClick(int mouseButton)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (mouseButton == 0)
                OnLeftMouseClick?.Invoke(hit);
            else if (mouseButton == 1)
                OnRightMouseClick?.Invoke(hit);
            else if (mouseButton == 2)
                OnMiddleMouseClick?.Invoke(hit);
        }
    }
}
