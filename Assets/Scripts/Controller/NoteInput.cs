using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteInput : MonoBehaviour
{
    public GameObject BlackClick;
    public GameObject RedClick;
    public GameObject WhiteClick;
    public GameObject Flick;
    public GameObject Hold;
    public GameObject Drag;

	public GameObject Note;
	public GameObject FatherObject;
	public static int index;
	Vector3 mousePosition, targetPosition;

	private void Awake()
    {
		Note = BlackClick;
    }
    private void Update()
    {
		if (Input.GetMouseButtonUp(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("touch area is UI");
			}
			else
			{
				mousePosition = Input.mousePosition;
				targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 14f));
				Note.transform.position = targetPosition;
				GameObject note=Instantiate(Note, Note.transform.position, Note.transform.rotation, FatherObject.transform);

			}
		}
	}
	Vector3 GetNotePos()
	{//×ø±ê×ª»»
		Vector3 imagePos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(Note.transform.parent as RectTransform, Input.mousePosition, null, out imagePos);
		return imagePos;
	}
	private void NoteChoose()
    {
        switch (index) {
			case 0:
                {
					Note = RedClick;
					break;
                }
			case 1:
				{
					Note = BlackClick;
					break;
                }
			case 2:
                {
					Note = WhiteClick;
					break;
                }
			case 3:
                {
					Note = Drag;
					break;
                }
			case 4:
                {
					Note = Hold;
					break;
                }
			case 5:
                {
					Note = Flick;
					break;
                }
		}
    }
	public void RedClickChoose()
    {
		index = 0;
		NoteChoose();
    }
	public void BlackClickChoose()
	{
		index = 1;
		NoteChoose();
	}
	public void whiteClickChoose()
	{
		index = 2;
		NoteChoose();
	}
	public void  DragChoose()
	{
		index = 3;
		NoteChoose();
	}
	public void HoldChoose()
	{
		index = 4;
		NoteChoose();
	}
	public void FlickChoose()
	{
		index = 5;
		NoteChoose();
	}
}
