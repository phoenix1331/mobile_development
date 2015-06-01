using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour {


	public GameObject missile;
	public int fire = 0;
	public GUISkin Fire;

	// Update is called once per frame
	/*void Update () {
		
		if (Input.GetMouseButtonDown (0))
		{
			Vector3 position = new Vector3(0f, 30.0f, 1f) * 10.0f;
			position = transform.TransformPoint (position);
			GameObject thisMissile = Instantiate (missile, position, transform.rotation) as GameObject;
			Physics.IgnoreCollision(thisMissile.collider, collider);
		}
		
		
	}*/
	void Update () {

		if (fire == 1)
		{
		    Vector3 position = new Vector3(0f,30.0f, 1f) * 10f;
			position = transform.TransformPoint (position);
			GameObject thisMissile = Instantiate (missile, position, transform.rotation) as GameObject;
			Physics.IgnoreCollision(thisMissile.collider, collider);
			fire = 0;
		}

	
	}

	public void OnGUI()
	{
		int butWidth, butHeight;
		
		butWidth = butHeight = 100;
		if(GUI.RepeatButton (new Rect (Screen.width-(butWidth+butWidth),Screen.height-butHeight,butWidth,butHeight), "",Fire.button))
		{
			fire = 1;
		}
}

}
