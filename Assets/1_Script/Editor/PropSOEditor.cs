using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Class
{
	[CustomEditor(typeof(StagePropsSO))]
	public class PropSOEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}