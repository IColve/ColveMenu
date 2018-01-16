//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;

public class MenuButtonView : MonoBehaviour 
{
	public delegate void menuButtonView();
	public event menuButtonView CloseWindowEvent;

	[SerializeField]
	private GameObject defaultButtonView;
	[SerializeField]
	private GameObject rootButton;

	private MenuButtonItem rootMenuButtonItem = new MenuButtonItem("Root");

	private List<MenuButtonItem> menuButtonList;

	[SerializeField]
	private GameObject backGround;

	public void Add(MenuButtonItem item,string parentName = null)
	{
		if(string.IsNullOrEmpty(item.rootButtonName))
		{
			if (menuButtonList == null) menuButtonList = new List<MenuButtonItem>();
			menuButtonList.ForEach(x =>
			{
				if (x.buttonName == item.buttonName)
				{
					Debug.Log("菜单父类名称重复");
					return;
				}
			});
			GameObject buttonObj = (GameObject)Instantiate(rootButton);
			buttonObj.transform.SetParent(transform);
			buttonObj.GetComponentInChildren<Text>().text = item.buttonName;
			buttonObj.SetActive(true);
			buttonObj.name = item.buttonName;
			menuButtonList.Add(item);
			buttonObj.GetComponent<Button>().onClick.AddListener(()=>
				{
					return;
					CloseWindow();
					MenuButtonItem data = FindMenuButtonItem(item.rootButtonName, item.buttonName);
					if(data != null && data.GetChilds() != null && data.GetChilds().Count != 0)
					{
						GameObject viewObj = CreateComponent(data, buttonObj.transform);
						viewObj.GetComponent<MenuButtonComponent>().SetPostion(new Vector2(0, -buttonObj.GetComponent<RectTransform>().sizeDelta.y));
					}
				});
		}
		else
		{
			MenuButtonItem parentItem = FindMenuButtonItem(item.rootButtonName, item.buttonName);
			if(parentItem != null)
			{
				parentItem.AddChild(item);
			}
			else
			{
				Debug.Log(parentName + "失踪");
			}
		}
	}

	public GameObject CreateComponent(MenuButtonItem data, Transform parent)
	{
		backGround.SetActive(true);
		GameObject viewObj = (GameObject)Instantiate(defaultButtonView);
		viewObj.transform.SetParent(parent);
		viewObj.SetActive(true);
		viewObj.GetComponent<MenuButtonComponent>().Init(data.GetChilds());
		return viewObj;
	}

	private MenuButtonItem FindMenuButtonItem(string rootButtonName, string buttonName)
	{
		MenuButtonItem menuButton = menuButtonList.First(x => x.buttonName == rootButtonName);
		return menuButton.buttonName == rootButtonName ? menuButton : FindMenuButtonItem(buttonName, menuButton);
	}

	private MenuButtonItem FindMenuButtonItem(string buttonName,MenuButtonItem item = null)
	{
		List<MenuButtonItem> itemList = item.GetChilds();
		if(itemList != null)
		{
			for (int i = 0; i < itemList.Count; i++) 
			{
				if(itemList[i].buttonName == buttonName) return itemList[i];
			}
			for (int i = 0; i < itemList.Count; i++) 
			{
				if(itemList[i] != null)
				{
					MenuButtonItem data = FindMenuButtonItem(buttonName, itemList[i]);
					if(data != null) return data;
				} 
			}
		}
		return null;
	}

	public void CloseWindow()
	{
		if(CloseWindowEvent != null) CloseWindowEvent.Invoke();
		if(backGround != null) backGround.SetActive(false);
	}
}

public class MenuButtonItem
{
	public string rootButtonName;
	public string buttonName;
	private Func<bool> enableCheckEvent;
	public bool hasLine;
	private UnityEvent buttonEvent;
	private List<MenuButtonItem> menuButtonList;

	public List<MenuButtonItem> GetChilds()
	{
		return menuButtonList;
	}

	public void AddChild(MenuButtonItem item)
	{
		if(menuButtonList == null)
		{
			menuButtonList = new List<MenuButtonItem>();
		}
		menuButtonList.Add(item); 
	}

	public void Remove(string actionName)
	{
		if(menuButtonList == null)
		{
			return;
		}
		foreach (var item in menuButtonList.Where(x => x.buttonName == actionName)) 
		{
			menuButtonList.Remove(item);
		}
	}

	public void GetClick()
	{
		if(buttonEvent != null) buttonEvent.Invoke();
	}

	public MenuButtonItem(string buttonName, string rootButtonName = "", UnityAction action = null, bool hasline = false, Func<bool> enableCheckEvent = null)
	{
		this.buttonName = buttonName;
		this.rootButtonName = rootButtonName;
		this.enableCheckEvent = enableCheckEvent == null ? () => true : enableCheckEvent;
		this.buttonEvent = new UnityEvent();
		this.hasLine = hasline;
		if(action != null)
		{
			this.buttonEvent.AddListener(action);
		}
	}
}