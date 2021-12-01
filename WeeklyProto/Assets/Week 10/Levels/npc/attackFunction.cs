using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LateExe;


public class attackFunction : MonoBehaviour{

    public float damage;

    public TrailRenderer[] trail;

    [Range(0,1)]public float maxDistance;
    public bool hasHit = true;


    string tt;

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position , transform.up , out hit , maxDistance))    {
            if(hit.transform.root.GetComponent<controller>() != null && !hasHit ){
                hit.transform.root.GetComponent<controller>().receiveDamage(damage);
                hasHit = true;
                Executer exe = new Executer(this);
                exe.DelayExecute(1.5f , x=> hasHit = false);
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
