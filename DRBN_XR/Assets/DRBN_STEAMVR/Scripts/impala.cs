using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impala : MonoBehaviour {

	float a=1.99f;
	Vector3 pos;
	float z_0=1.575f; //nm
	float Trigger_z;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		Trigger_z = this.gameObject.transform.position.y;
		Debug.Log(this.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float CalcdCz(float z)
	{
	   var x = z-Trigger_z;
	   var exp_term = Mathf.Exp (a * (Mathf.Abs(x) - z_0));
	   
	   // C_z = 1 - 1/(1+Mathf.Exp (a * (Mathf.Abs(z-Trigger_z) - z_0)));
	   var dC_z = (a*x*exp_term)/((exp_term+1)*(exp_term+1)*Mathf.Abs(x));
	   return dC_z;
	}


	void OnTriggerStay (Collider collider) {
	        Vector3 up = new Vector3 (0f, 1f, 0f);
	        var hydrophobicity = collider.gameObject.transform.parent.parent.GetComponent<DomainProperties>().getHydrophobicity();
	        var area = collider.gameObject.transform.parent.parent.GetComponent<DomainProperties>().getArea();
		pos = collider.gameObject.transform.position;
		var f = (18*area-hydrophobicity)*CalcdCz(pos.y)*up;//every collider is a grandchild of the domain rigidbody
		rb = collider.gameObject.transform.parent.parent.GetComponent<Rigidbody> ();
		rb.AddForceAtPosition (f, pos);
		
	}
}
