using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using System.IO;

public class Langevin_v3 : MonoBehaviour {

    public List<Rigidbody> GOS;
    public List<GameObject> GOmol;
    
    /*Langevin variables*/
    public float temp = 300.0f;
    public static float kB = 8.31f; // Distance:nm, Time:ns, Mass:,kDa, Temperature:K 
    public float friction = 28.655f;
    public static float dt = GlobalVars.dt;
    
  
    

    //make a list of objects that are tagged with the "molecule" tag
     (List<Rigidbody>,List<GameObject>) CountObjects()
    {
	GOmol = GameObject.FindGameObjectsWithTag("molecule").ToList<GameObject>();
        foreach (GameObject go in GOmol)
            {GOS.AddRange(go.GetComponentsInChildren<Rigidbody>());}
        
        return (GOS,GOmol);
    }
    void integrate(Rigidbody domain)
    {
        MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0,1);
        float coll_constant = friction/domain.mass;
        float sigma = (float)Math.Sqrt(2*kB*temp*coll_constant/domain.mass);
        Vector3 randomVector = new Vector3((float)normalDist.Sample(),(float)normalDist.Sample(),(float)normalDist.Sample());
        domain.AddForce((float)Math.Sqrt(dt)*sigma*randomVector-dt*coll_constant*domain.linearVelocity,ForceMode.VelocityChange);
    }
    
    
    void OnGUI()
    {
        var t = GlobalVars.i*dt;
        GUI.Label(new Rect(0, 0, 1000, 100), "time " + t.ToString() + "ns");
    }

    void LangevinSolver()
    {
      foreach (Rigidbody GO in GOS)
      {    
         integrate(GO);
      }
    }
    void PrintTemperature()
    {
      float temperature;
      float kin_eng = 0;
      int n_domains = GOS.Count;
      foreach (Rigidbody GO in GOS)
      {
         kin_eng += 0.5f*GO.mass*Vector3.Dot(GO.linearVelocity,GO.linearVelocity);
      }
      temperature = 2*kin_eng / ((3*n_domains-3)*kB);
      Debug.Log(temperature);
   
    }
    
    void Start()
    {
        Debug.Log(Time.fixedDeltaTime);
        CountObjects();
        foreach (Rigidbody GO in GOS)
        {
          var std = Math.Sqrt(kB*temp/GO.mass);
          MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0,std);
          Vector3 newVelocity = new Vector3((float)normalDist.Sample(),(float)normalDist.Sample(),(float)normalDist.Sample());
          GO.AddForce(newVelocity,ForceMode.VelocityChange);
        }
    }
    void FixedUpdate ()
    {   
        if (GlobalVars.i > 0)
        {
          LangevinSolver();
        }
        if (GlobalVars.i % 500 == 0 || GlobalVars.i == 0)
        {
            PrintTemperature();
        }
     if (GlobalVars.i >= 600000)
     { 
       UnityEditor.EditorApplication.isPlaying = false;
      }
    }
}
