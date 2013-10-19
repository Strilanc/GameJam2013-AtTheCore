using UnityEngine;
using System.Collections;

public class OcculusQuestionmark : MonoBehaviour {
	
	public static bool hasOcculus;
	public Rect rect;
	public void OnGUI(){	
		GUI.BeginGroup(rect);
		
		if(GUILayout.Button("I have an occulus")){
			hasOcculus = true;
			Application.LoadLevel("Scene1");
		}
		if(GUILayout.Button("I do not have an occulus")){
			hasOcculus = false;
			Application.LoadLevel("Scene1");
		}
		
		
		GUI.EndGroup();
		
	}
}
