using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    internal DropTray tray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        tray.MouseHovering(this);
    }

    void OnMouseExit()
    {
        tray.MouseStopHovering(this);
    }
}
