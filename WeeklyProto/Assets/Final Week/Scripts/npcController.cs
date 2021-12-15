using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LateExe;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class npcController : MonoBehaviour{
    [Header("components")]
    //public Slider healthSlider;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private GameObject playerObject;

    [Header("values")]
    public float roamCooldownTime = 10;
    public float maxRoamDistance;
    public float health;
    public float attackCooldown;
    public float attackTimer;
    // public GameObject growl;
    //public GameObject growl2;
    

    [Header("debug")]

    [Header("private")]
    [HideInInspector]public int characterState;
    private float horizontal , vertical;
    private float RoamTimer;
    private float Velocity = 0;
    private bool stateChangedFlag = false;//used once to change state of player
    private bool attacking;
    private bool hasDied;


    void Start(){
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject == null)print("no object with player tag");

       // healthSlider.maxValue = health;
    }

    void Update(){

        if(hasDied)return;

        if(health < 1 && !hasDied){
            hasDied = true;
            animator.SetTrigger("die");
           // healthSlider.transform.parent.gameObject.SetActive(false);
            // growl.SetActive(false);
            // growl2.SetActive(true);
            
        }

       

       // healthSlider.value = health;
        
        if(characterState == 0){
            Roam();
        }else{
            combat();
        }
    }
    
    void combat(){
        if(getPlayerDistance() < 2){
            attack();
        }else{
            getCloserToPlayer();
        }
    }

    void getCloserToPlayer(){
        if(!attacking)
        navMeshAgent.SetDestination(playerObject.transform.position);
    }

    void attack(){
        if(!attacking){
            animator.SetTrigger("attack");
            transform.LookAt(playerObject.transform);
            Executer exe = new Executer(this);
            exe.DelayExecute(attackCooldown , x=> attacking = false);
        }
    }

    void FixedUpdate(){
        animations();
        checkNPC();    
    }

    void checkNPC(){
        if(getPlayerDistance() < 20 && !stateChangedFlag){
            stateChangedFlag = true ;
            Executer exe = new Executer(this);
            exe.DelayExecute(3 , x=> setState(1));
            
        }
        if(getPlayerDistance() > 20 && stateChangedFlag){
            stateChangedFlag = false ;
            Executer exe = new Executer(this);
            exe.DelayExecute(8 , x=> setState(0));
        }


    }

    void setState(int value){
        characterState = value;
        playerObject.GetComponent<controller>().characterState = value;
        playerObject.GetComponent<controller>().equipMele();
    }

    void Roam(){
        if(characterState != 0)return;
        
        if(Time.time > RoamTimer){
            float a = Random.Range(0,2);
            RoamTimer = Time.time + roamCooldownTime;
            navMeshAgent.SetDestination(new Vector3(transform.position.x +  Random.Range(0 , maxRoamDistance) * (a == 1 ?  1 : -1),0
            ,transform.position.z +  Random.Range(0 , maxRoamDistance) * (a == 1 ?  1 : -1)));
        }


    }

    void animations(){
        vertical = Mathf.Lerp(vertical , navMeshAgent.remainingDistance > 0 ? 1 : 0 , 5 * Time.deltaTime);
        animator.SetFloat("vertical", vertical);
        animator.SetFloat("horizontal", horizontal);
        animator.SetInteger("state", characterState);
    }

    string getCharState(){
        switch(characterState){
            case 0:
                return "peaceful";
            case 1:
                return "Combat";
        }
        return"out of range";
        
    }
    
    float getPlayerDistance(){
        if(playerObject != null){
            return Vector3.Distance(transform.position , playerObject.transform.position);
        }
        return 0;
    }

    public void receiveDamage(float value){
        animator.SetTrigger("takeDamage");
        health -= value;
    }

    void OnGUI(){
        GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"NPC acceleration: " +  navMeshAgent.remainingDistance);
        PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);

        GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"NPC state: " +  getCharState());
        PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);
      
        GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"NPC state: " +  getPlayerDistance().ToString("0.00"));
        PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);


    }
}
    