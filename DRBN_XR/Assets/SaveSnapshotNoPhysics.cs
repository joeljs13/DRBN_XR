using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;



public class SaveSnapShotNoPhysics : MonoBehaviour
{
    int period = 500;
    void FixedUpdate()
    {
        if (GlobalVars.i % period == 0 || GlobalVars.i == 0)
        {
            periodicsaveJSON((int)GlobalVars.i);
        }
    }

    public void periodicsaveJSON(int step)
    {
        List<SavedataHierarchy> savefile = new List<SavedataHierarchy>();

        List<Rigidbody> RBlist = new List<Rigidbody>();

        List<Coords> GOVertscoords = new List<Coords>();
        
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("molecule");
        foreach(var molecule in gameObjects)
        {
            Rigidbody[] rigidBodies = molecule.GetComponentsInChildren<Rigidbody>();
        
            foreach(var rigidBody in rigidBodies)
            {
                savefile.Add(new SavedataHierarchy(rigidBody.transform.parent.name,rigidBody.name, rigidBody.position, rigidBody.rotation));
            }
        }

        
        RBListContainerHierarchy container = new RBListContainerHierarchy(savefile);
        string json = JsonUtility.ToJson(container, true);

        var t = GlobalVars.i;
        
        File.WriteAllText(GlobalVars.pathTrajectoryFolder+"/gamesave_list_"+ t.ToString() + "ns.jsonbrn", json);

    }
}
