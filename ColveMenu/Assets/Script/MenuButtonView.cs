//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
using UnityEngine.UI;

namespace SceneStudio {
	public class MenuButtonView : MonoBehaviour 
	{
		public delegate void menuButtonView();
		public event menuButtonView CloseWindowEvent;

		[SerializeField]
		private GameObject defaultButtonView;
		[SerializeField]
		private GameObject rootButton;

		private MenuButtonItem rootMenuButtonItem = new MenuButtonItem("Root"); 

		[SerializeField]
		private GameObject backGround;

		public void Add(MenuButtonItem item,string parentName = null)
		{
			if(string.IsNullOrEmpty(parentName))
			{
				GameObject buttonObj = (GameObject)Instantiate(rootButton);
				buttonObj.transform.SetParent(transform);
				buttonObj.GetComponentInChildren<Text>().text = item.actionName;
				buttonObj.SetActive(true);
				buttonObj.name = item.actionName;
				rootMenuButtonItem.AddChild(item);
				buttonObj.GetComponent<Button>().onClick.AddListener(()=>
					{
						CloseWindow();
						MenuButtonItem data = Find(item.actionName);
						if(data != null && data.GetChilds() != null && data.GetChilds().Count != 0)
						{
							GameObject viewObj = CreateComponent(data, buttonObj.transform);
							viewObj.GetComponent<MenuButtonComponent>().SetPostion(new Vector2(0, -buttonObj.GetComponent<RectTransform>().sizeDelta.y));
						}
					});
			}
			else
			{
				MenuButtonItem parentItem = Find(parentName);
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

		private MenuButtonItem Find(string name,MenuButtonItem item = null)
		{
			if(item == null) item = rootMenuButtonItem;
			List<MenuButtonItem> itemList = item.GetChilds();
			if(itemList != null)
			{
				for (int i = 0; i < itemList.Count; i++) 
				{
					if(itemList[i].actionName == name) return itemList[i];
				}
				for (int i = 0; i < itemList.Count; i++) 
				{
					if(itemList[i] != null)
					{
						MenuButtonItem data = Find(name, itemList[i]);
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
		[SerializeField]
		public string actionName;

		private Func<bool> enableCheckEvent;

		public bool hasLine;

		[SerializeField]
		private UnityEvent buttonEvent;
		[SerializeField]
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
			foreach (var item in menuButtonList.Where(x => x.actionName == actionName)) 
			{
				menuButtonList.Remove(item);
			}
		}

		public void GetClick()
		{
			if(buttonEvent != null) buttonEvent.Invoke();
		}

		public MenuButtonItem(string actionName, UnityAction action = null, bool hasline = false, Func<bool> enableCheckEvent = null)
		{
			this.actionName = actionName;
			this.enableCheckEvent = enableCheckEvent == null ? () => true : enableCheckEvent;
			this.buttonEvent = new UnityEvent();
			this.hasLine = hasline;
			if(action != null)
			{
				this.buttonEvent.AddListener(action);
			}
		}
	}
}