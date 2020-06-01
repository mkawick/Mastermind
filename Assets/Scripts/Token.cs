using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public bool isClickingEnabled = true;
    //[SerializeField]
    internal OptionsTray optionsTray;
    public Vector3 originalPosition;
    public int choiceIndex
    {
        get;
        set;
    }
    Vector3 offset, screenPoint;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // ------------------------------------------------------------
    void Update()
    {
        if (isClickingEnabled == false)
            return;

    }
    private void OnMouseDown()
    {
        if (isClickingEnabled == false)
            return;

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        this.GetComponent<SphereCollider>().enabled = false;
    }
    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
    private void OnMouseUp()
    {
        this.GetComponent<SphereCollider>().enabled = true;
        if (isClickingEnabled == false)
            return;

        if (optionsTray != null)
        {
            if(optionsTray.SuccessfulDrop(this))
            {
                return;
            }
        }

        iTween.MoveTo(this.gameObject, originalPosition, 0.5f);


        //isMouseButtonDown = false;
    }

    void UpdateWorldPOsitionBasedOnMouse()
    {
        Vector3 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = this.transform.position.y;
        this.transform.position = mousePos;
    }
}
