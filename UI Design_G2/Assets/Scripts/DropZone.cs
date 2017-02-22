using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour,IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log ("OnPointerEnter");
		if (eventData.pointerDrag == null)
			return;
		CardDraggable d= eventData.pointerDrag.GetComponent<CardDraggable> ();
		if (d != null) 
		{
			d.placeholderParent = this.transform;
		}
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log ("OnPointerExit");
		if (eventData.pointerDrag == null)
			return;
		CardDraggable d= eventData.pointerDrag.GetComponent<CardDraggable> ();
		if (d != null&&d.placeholderParent==this.transform) 
		{
			d.placeholderParent = d.parentToReturnTo;
		}

	}
	public void OnDrop(PointerEventData eventData)
	{
		Debug.Log (eventData.pointerDrag.name + "Was Dropped On" + gameObject.name);
		//Draggable d=eventData.pointerDrag.GetComponent<Draggable> ();


		/*if (d != null) 
		{
			d.parentToReturnTo = this.transform;
		}*/
		CardDraggable d= eventData.pointerDrag.GetComponent<CardDraggable> ();
		if (d != null) 
		{
			d.parentToReturnTo = this.transform;
		}
	}

}
