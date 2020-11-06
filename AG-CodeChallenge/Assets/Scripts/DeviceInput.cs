using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeviceInput : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_IOS || UNITY_ANDROID
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.tapCount == 2)
            {
                Debug.Log("Mobile Double tap");
                GetComponent<ICreatePolygon>().ChangeRandomColor();
            }
        }        
#else
        if (eventData.clickCount == 2)
        {
            Debug.Log("Double click");
            GetComponent<ICreatePolygon>().ChangeRandomColor();
        }
#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name);
    }
}
