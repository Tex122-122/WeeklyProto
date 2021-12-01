using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleStats : MonoBehaviour{

    public float damage;

    public TrailRenderer[] trail;

    [Range(0,1)]public float maxDistance;
    public bool hasHit = true;


    string tt;

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position , transform.up , out hit , maxDistance))    {
            if(hit.transform.root.GetComponent<npcController>() != null && !hasHit ){
                hit.transform.root.GetComponent<npcController>().receiveDamage(damage);
                hasHit = true;
            }
        }
    }

    public void setTrailEmision(bool val){
        if(trail != null ){
            foreach (var item in trail){
                item.emitting = val;
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawRay(transform.position , transform.up * maxDistance);
    }

    void OnGUI(){
        GUI.Label(new Rect(180, 50,200,20 ),tt );

    }
}
