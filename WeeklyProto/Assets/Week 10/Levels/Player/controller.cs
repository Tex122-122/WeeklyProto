using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LateExe;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class controller : MonoBehaviour{

    [Header("character components")]
    public GameObject focusPoint;
    [Header("character weapon")]
    public GameObject handPos;
    public GameObject backPos;
    private GameObject mele;
    private gameManager manager;
    private InputManager inputManager;
    private CharacterController characterController;
    private Animator animator;
    public Slider healthSlider;
    public GameObject player;
    //public GameObject deathCam;
    //public GameObject gameover;
    public GameObject ui;
    public GameObject music;
    public GameObject deathCam;
    public GameObject gameOver;
    




    [Header("character Values")]
    public int characterState = 0;
    public float health = 100;
    [Range(1,4)]public float movementSpeed = 2;
    [Range(0,.5f)]public float groundClearance;
    [Range(0,1)]public float groundDistance;
    
    [Header("combat Values")]
    [Range(0,4)]public float combatSpeed;
    [Range(0,1)]public float lightattackTimer;
    public bool lightAttack;
    public bool heavyAttack;
    public bool takingDamage;
    [Range(0,1)]public float takingDamageTimer;
    [Range(0,1)]public float heavyattackTimer;
    [Range(0,1)]public float rollTimer;
    [Range(0,4)]public float rollspeed;

    [Header("debug Values")]
    public float jumpValue = -9.8f;

    [Header("private Values")]
    private Vector3 motionVector , gravityVector ;
    private Vector3 relativeVector; // used for mouse look
    private float turnAmount = 90;
    private float gravityPower = -9.8f;
    private bool cursorLoced = true;
    private float gravityForce = -9.18f;
    private float jumpTimer;
    private float attackCooldown;
    public bool rolling;
    public float turnDirection;
    private controller script;
    

    void Start(){
        inputManager = GetComponent<InputManager>();    
        characterController = GetComponent<CharacterController>();    
        animator = GetComponent<Animator>();
        manager = FindObjectOfType<gameManager>();
        if(manager == null) print("No manager in scene");
        spawnWeapon();
        mele.GetComponent<meleStats>().setTrailEmision(false);
        healthSlider.maxValue = health;
       
    }

    void Update(){
        playerStates();

        mouseLook();
        healthSlider.value = health;

        if (health <= 0)
        {
            player.SetActive(false);
            deathCam.SetActive(true);
            ui.SetActive(false);
            gameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            music.SetActive(false);
            animator.SetTrigger("die");
            script.enabled = !script.enabled;
        }
        
    }

    public void equipMele(){
        if(characterState == 0){
            mele.transform.parent = backPos.transform;
            mele.transform.localPosition = Vector3.zero;
            mele.transform.localEulerAngles = Vector3.zero;
            mele.transform.localScale = new Vector3(1,1,1);
        }else{
            mele.transform.parent = handPos.transform;
            mele.transform.localPosition = Vector3.zero;
            mele.transform.localEulerAngles = Vector3.zero;
            mele.transform.localScale = new Vector3(1,1,1);
        }
    }

    void spawnWeapon(){
        mele = Instantiate(manager.meleWeapons.GetComponent<meleWeapons>().weapons[PlayerPrefs.GetInt("currentWeaponIndex")]);
        equipMele();
    }

    void mouseLook(){
        if(Input.GetKeyDown(KeyCode.Tab))cursorLoced = (cursorLoced)? false : true;
        Cursor.lockState = cursorLoced ? CursorLockMode.Locked : CursorLockMode.None ;
        if(cursorLoced ){
            relativeVector = transform.InverseTransformPoint(focusPoint.transform.position + (focusPoint.transform.forward * 5));
            relativeVector /= relativeVector.magnitude;
            turnDirection = (relativeVector.x  / relativeVector.magnitude) ;

            //vertical
            focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + -Input.GetAxis("Mouse Y") ,focusPoint.transform.eulerAngles.y ,0) ;
            //horizontal
            focusPoint.transform.parent.Rotate(transform.up * Input.GetAxis("Mouse X") * 100 * Time.deltaTime );

                 
        }
        
    }

    void combatMovement(){

        if(isGrounded() && !lightAttack && !heavyAttack && !rolling){
            motionVector = transform.right * inputManager.rawHorizontal + transform.forward * inputManager.rawVertical * (1+ (inputManager.rawVertical > 0 ? inputManager.shift : 0)); 
            characterController.Move(motionVector * combatSpeed * Time.deltaTime);

            if(inputManager.rawHorizontal != 0 || inputManager.rawVertical != 0){
                focusPoint.transform.parent.Rotate(transform.up * -turnDirection * 400 * Time.deltaTime );
                transform.Rotate(transform.up * turnDirection * 400 * Time.deltaTime );
            }
        }

        if(rolling){
            motionVector = transform.right * inputManager.rawHorizontal + transform.forward * 2.7f ; 
            characterController.Move(motionVector * rollspeed * Time.deltaTime);
        }

        //rolling function >
        if(inputManager.jump == 1 && !rolling && !lightAttack && !heavyAttack)
        if(inputManager.rawVertical != 0 || inputManager.rawHorizontal != 0  &&  Time.time > attackCooldown){
            rolling = true;
            animator.SetTrigger("roll");
            print("roll");
            transform.Rotate(transform.up * inputManager.horizontal * 90);
            Executer exe = new Executer(this);
            exe.DelayExecute(rollTimer , x=> rolling = false);
        }

        //light attact >>
        if(!lightAttack && !rolling && inputManager.fire == 1){
            animator.SetTrigger("attack");
            mele.GetComponent<meleStats>().hasHit = false;
            lightAttack = true;
            Executer aa = new Executer(this);
            aa.DelayExecute(lightattackTimer , x=> lightAttack = false );
        }

        //heavy attact >>
        if(!heavyAttack && !rolling && inputManager.fire1 == 1){
            animator.SetTrigger("attackHeavy");
            mele.GetComponent<meleStats>().hasHit = false;
            heavyAttack = true;
            Executer bb = new Executer(this);
            bb.DelayExecute(heavyattackTimer , x=> heavyAttack = false );
        }
        

        mele.GetComponent<meleStats>().setTrailEmision(lightAttack || heavyAttack ? true : false);

        animator.SetBool("grounded",isGrounded());
        animator.SetFloat("horizontal",inputManager.horizontal);
        animator.SetFloat("vertical",inputManager.rawVertical  * (1+ (inputManager.rawVertical > 0 ? inputManager.shift : 0))); 
        animator.SetInteger("state",characterState);

        if(isGrounded() && gravityVector.y < 0)
            gravityVector.y = -2;

        gravityVector.y += gravityPower * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);
    }

    public void receiveDamage(float value){
        if(!takingDamage){
            health -= value;
            animator.SetTrigger("takeDamage");
            Executer bb = new Executer(this);
            bb.DelayExecute(takingDamageTimer , x=> takingDamage = false );
        }
        
    }

    void peacefulMovement(){

        if(isGrounded() && gravityVector.y < 0)
            gravityVector.y = -2;

        gravityVector.y += gravityPower * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);

        if(inputManager.jump != 0 && isGrounded()){ 
            if(Time.time > jumpTimer){
                animator.SetTrigger("jump");
                Executer exe = new Executer(this);
                exe.DelayExecute(.1f , x=> jump());
                jumpTimer = Time.time + 1.1f;
                inputManager.vertical = 1;
            }         
        }

        motionVector = transform.right * (inputManager.vertical > 1 ? inputManager.horizontal * 2 : inputManager.horizontal ) + transform.forward * inputManager.vertical * (1 + inputManager.shift);
        if(isGrounded()){
            if(inputManager.vertical > 0 ){
                //characterController.Move(motionVector * movementSpeed * Time.deltaTime);
                transform.Rotate(transform.up * turnDirection * 400 * Time.deltaTime );
                focusPoint.transform.parent.Rotate(transform.up * -turnDirection * 400 * Time.deltaTime );
            }
            if(inputManager.rawHorizontal != 0 && inputManager.rawVertical == 0 ){
                focusPoint.transform.parent.Rotate(transform.up * inputManager.horizontal / 2 * 100 * Time.deltaTime );                
            }

        }
        characterController.Move(motionVector * movementSpeed * Time.deltaTime);
        

        animator.SetBool("grounded",isGrounded());
        animator.SetFloat("horizontal",inputManager.rawVertical != 0 ? inputManager.horizontal / 2 : inputManager.horizontal);
        animator.SetFloat("vertical",inputManager.vertical);
        animator.SetInteger("state",characterState);
        if(inputManager.fire != 0 && Time.time > attackCooldown && characterState == 1){
            animator.SetTrigger("attack");
            attackCooldown = Time.time + 1.4f;
            mele.GetComponent<meleStats>().hasHit = false;
        }


    }

    void jump(){
        characterController.Move(transform.up * (jumpValue * -2 * gravityForce) * Time.deltaTime);
    }

    bool isGrounded(){
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z) , groundClearance);
    }

    void playerStates(){
        if(characterState == 0 && !takingDamage){
                peacefulMovement();
        }
        else{
                combatMovement();
        }
    }

    
    // void OnDrawGizmosSelected(){
    //Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y - groundDistance,transform.position.z),groundClearance);    
    //}

    //void OnGUI(){
    //PlayerPrefs.SetInt("rectPos",50);

    //GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"is Grounded: " +  isGrounded());
    // PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);

    //GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"player state: " +  characterState);
    //PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);

    // GUI.Label(new Rect(20, PlayerPrefs.GetInt("rectPos"),200,20 ),"cursorLocked: " +  cursorLoced);
    //PlayerPrefs.SetInt("rectPos",PlayerPrefs.GetInt("rectPos") + 30);

    //}

}   
