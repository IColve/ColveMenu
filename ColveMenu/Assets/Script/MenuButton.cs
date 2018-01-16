//Author: Colve

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuButton : Button 
{
	public UnityAction action;

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if(state == SelectionState.Highlighted && action != null)
		{
			action.Invoke();
		}
		base.DoStateTransition(state,instant);
	}
}