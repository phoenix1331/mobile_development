using UnityEngine;
using System.Collections;

public class CarControlScript : MonoBehaviour {
	public WheelCollider WheelFL;
	public WheelCollider WheelFR;
	public WheelCollider WheelRL;
	public WheelCollider WheelRR;
	
	public Transform WheelFLTrans;
	public Transform WheelFRTrans;
	public Transform WheelRLTrans;
	public Transform WheelRRTrans;
	
	float lowestSteerAtSpeed = 50;
	float lowSpeedSteerAngle = 10;
	float highSpeedSteerAngle = 1;
	
	float decelerationSpeed = 30;
	public float currentSpeed;
	float topSpeed = 100;
	
	private float maxTorque = 50;
	private int gasLevel;
	private int brakeLevel=0;
	
	public GameObject truckBody;
	public Material normalMat;
	public Material brakeMat;
	public Material reverseMat;

	public GUISkin UpArrow;
	public GUISkin DownArrow;
	public GUISkin Brake;
	
	
	// Use this for initialization
	void Start () {
		rigidbody.centerOfMass = new Vector3(0,0,0);
		gasLevel = 0;
		brakeLevel = 0;
	}
	
	// Update is called once per frame
	void Update () {
		WheelFLTrans.Rotate(WheelFL.rpm/60*360*Time.deltaTime,0,0);
		WheelFRTrans.Rotate(WheelFR.rpm/60*360*Time.deltaTime,0,0);
		WheelRLTrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime,0,0);
		WheelRRTrans.Rotate(WheelRR.rpm/60*360*Time.deltaTime,0,0);
		
		float ttFL = WheelFL.steerAngle - WheelFLTrans.localEulerAngles.z;
		float ttFR = WheelFR.steerAngle - WheelFRTrans.localEulerAngles.z;
		Vector3 AngleFL = new Vector3 (WheelFLTrans.localEulerAngles.x,ttFL,WheelFLTrans.localEulerAngles.z); WheelFLTrans.localEulerAngles = AngleFL;
		Vector3 AngleFR = new Vector3 (WheelFRTrans.localEulerAngles.x,ttFR,WheelFRTrans.localEulerAngles.z); WheelFRTrans.localEulerAngles = AngleFR;
		BrakeLight();
		EngineSound();
	}
	
	void FixedUpdate () {
		currentSpeed = (2*3.1415926f *WheelRL.radius*WheelRL.rpm*60/1000); 
		currentSpeed = Mathf.Round(currentSpeed);
		if(currentSpeed < topSpeed)
		{
			WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical"); 
		    WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");
			
			WheelRR.motorTorque = maxTorque * gasLevel; 
			WheelRL.motorTorque = maxTorque * gasLevel;
		}
		else
		{
			WheelRR.motorTorque = 0;
			WheelRL.motorTorque = 0;
		}
		
		if(Input.GetButton("Vertical")==false) //if no gas, slow down the car
		{
			WheelRR.brakeTorque = decelerationSpeed;
			WheelRL.brakeTorque = decelerationSpeed;
		}
		else
		{
			WheelRR.brakeTorque = 0;
			WheelRL.brakeTorque = 0;
		}
		
		if (brakeLevel > 0 ) 
		{
			WheelRR.brakeTorque = 80;
			WheelRL.brakeTorque = 80;
		} 
		else if (gasLevel==0)
		{
			WheelRR.brakeTorque = 30;
			WheelRL.brakeTorque = 30;
		}
		else 
		{
			WheelRR.brakeTorque = 0;
			WheelRL.brakeTorque = 0;
		}
		
		Vector3 accelerator = Input.acceleration;
		
		float speedFactor = rigidbody.velocity.magnitude/lowestSteerAtSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor); 
		currentSteerAngle *= Input.GetAxis("Horizontal");
		//currentSteerAngle *= accelerator.x*2.0f;
		WheelFL.steerAngle = currentSteerAngle;
		WheelFR.steerAngle = currentSteerAngle;
	}
	
	public void OnGUI()
	{
		int butWidth, butHeight;
		GUIStyle myButtonStyleBig = new GUIStyle (GUI.skin.button);
		myButtonStyleBig.fontSize = 30;


		
		butWidth = butHeight = 100;
		//Forward
		if(GUI.RepeatButton (new Rect (0,Screen.height-(butHeight+butHeight),100,100), "", UpArrow.button))
		{
			gasLevel =5;
		}
		else if(GUI.RepeatButton (new Rect (0,Screen.height-(butHeight),100,100), "", DownArrow.button))
		{
			gasLevel =-5;

		}
		else gasLevel =0;
		
		if(GUI.RepeatButton (new Rect (Screen.width-butWidth,Screen.height-butHeight,butWidth,butHeight), "",Brake.button))
		{
			brakeLevel =5;

		}
		else brakeLevel =0;
		
		/*if(GUI.RepeatButton (new Rect (Screen.width-butWidth,Screen.height-butHeight,butWidth,butHeight), "Reverse",myButtonStyleBig))
		{
			gasLevel =-5;
		}*/


	}
	
	void BrakeLight()
	{
		if(currentSpeed > 0 && Input.GetAxis ("Vertical") < 0 )
			truckBody.renderer.material = brakeMat;
		else if(currentSpeed < 0 && Input.GetAxis ("Vertical") > 0 )
			truckBody.renderer.material = brakeMat;
		else if(currentSpeed < 0 && Input.GetAxis ("Vertical") < 0 )
			truckBody.renderer.material = reverseMat;
		else
			truckBody.renderer.material = normalMat;
		
		if(brakeLevel>0 )
			truckBody.renderer.material = brakeMat;
		else
			truckBody.renderer.material = normalMat;
	}
	
	void EngineSound()
	{
		if(currentSpeed > 0 ){
			audio.pitch = (currentSpeed/2)/topSpeed+1;
		}
	}
	
	
	
}
