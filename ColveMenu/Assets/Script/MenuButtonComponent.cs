//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuButtonComponent : MonoBehaviour 
{
	[SerializeField]
	private GameObject defaultButton;
	[SerializeField]
	private GameObject lineObj;
	[SerializeField]
	private MenuButtonView menuButtonView;
	private MenuButtonComponent childView;

	void Start()
	{
		menuButtonView.CloseWindowEvent += DestroyWindow;
	}

	public void Init(List<MenuButtonItem> items)
	{
		this.gameObject.SetActive(true);
		items.ForEach(x => CreateButton(x));
	}

	public void CreateButton(MenuButtonItem item)
	{
		if(item == null) return;
		GameObject buttonObj = (GameObject)Instantiate(defaultButton);
		buttonObj.transform.SetParent(transform);
		buttonObj.SetActive(true);
		buttonObj.GetComponent<MenuButton>().onClick.AddListener(() =>
			{
				item.GetClick();
				menuButtonView.CloseWindow();
			});
		buttonObj.GetComponent<MenuButton>().action = () =>
		{
			ClearChild();
			if(item.GetChilds() != null && item.GetChilds().Count != 0)
			{
				GameObject viewObj = menuButtonView.CreateComponent(item, buttonObj.transform);
				childView = viewObj.GetComponent<MenuButtonComponent>();
				viewObj.GetComponent<MenuButtonComponent>().SetPostion(new Vector2(buttonObj.GetComponent<RectTransform>().sizeDelta.x, 0));
			}
		};
		buttonObj.GetComponentInChildren<Text>().text = item.buttonName;
		buttonObj.name = item.buttonName;
		if(item.hasLine)
		{
			GameObject line = (GameObject)Instantiate(lineObj);
			line.transform.SetParent(transform);
			line.SetActive(true);
		}

	}

	public void ClearChild()
	{
		if(childView != null)
		{
			childView.ClearChild();
			childView.DestroyWindow();
		}	
	}

	public void DestroyWindow()
	{
		menuButtonView.CloseWindowEvent -= DestroyWindow;
		Destroy(gameObject);
	}

	public void SetPostion(Vector2 vec2)
	{
		GetComponent<RectTransform>().anchoredPosition = vec2;
	}
}