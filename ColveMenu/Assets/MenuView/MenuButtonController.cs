//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace SceneStudio {
	public class MenuButtonController : MonoBehaviour 
	{
		public List<EditorMenuButtonItem> menuButtonList;
		public List<EditorMenuButtonItem> menuButtonItemList;
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
						MenuButtonItem item = new MenuButtonItem(x.actionName);
						menuButtonView.Add(item, x.parentName);
					});
			}
			if(menuButtonItemList != null)
			{
				menuButtonItemList.ForEach(x => 
					{
						MenuButtonItem item = new MenuButtonItem(x.actionName, () => x.buttonEvent.Invoke(), x.hasline);
						menuButtonView.Add(item, x.parentName);
					});
			}
		}

	}


	[System.Serializable]
	public class EditorMenuButtonItem
	{
		public string parentName;
		public string actionName;
		public bool hasline;
		public UnityEvent buttonEvent;
	}
}