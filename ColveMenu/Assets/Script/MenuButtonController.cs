//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class MenuButtonController : MonoBehaviour
{
	public List<EditorMenuButton> menuButtonList;
	public MenuButtonView menuButtonView;

	void Start()
	{
		if(menuButtonView == null)
		{
			menuButtonView = GameObject.FindObjectOfType<MenuButtonView>();
		}
		if(menuButtonList != null)
		{
			menuButtonList.ForEach(x =>
			{
				MenuButtonItem menuButton = new MenuButtonItem(x.buttonName);
				menuButtonView.Add(menuButton);
				if (x.menuButtonitemList != null)
				{
					x.menuButtonitemList.ForEach(y =>
					{
						MenuButtonItem menuButtonItem = new MenuButtonItem(y.buttonName, x.buttonName, () => y.buttonEvent.Invoke(), y.hasline);
						menuButtonView.Add(menuButtonItem, y.parentButtonName);
					});
				}
			});
		}
	}

}

[System.Serializable]
public class EditorMenuButtonItem
{
	public string parentButtonName;
	public string buttonName;
	public bool hasline;
	public UnityEvent buttonEvent;
}

[System.Serializable]
public class EditorMenuButton
{
	public string buttonName;
	public List<EditorMenuButtonItem> menuButtonitemList;
}