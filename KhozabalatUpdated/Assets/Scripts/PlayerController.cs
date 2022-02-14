using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public Rigidbody RB;
    
    public float moveSpeed = 10f;

    public Camera viewCam;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float mouseSenitivity = 1f;

    private float maxAngle = 160f;
    private float minAngle = 10f;

    public int currentHealth;
    public GameObject deathScreen;

    public Animator gunAnim;
    
    
    // Start is called before the first frame update
    void Start(){
        LockCursor();
        instance = this;
    }

    // Update is called once per frame
    void Update(){
        // Player Movement
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;
        RB.velocity = (moveHorizontal + moveVertical) * moveSpeed;

        // Player POV
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSenitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);
        Vector3 RotAmount = viewCam.transform.localRotation.eulerAngles + new Vector3 (0f, mouseInput.y, 0f);
        viewCam.transform.localRotation = Quaternion.Euler(RotAmount.x, Mathf.Clamp(RotAmount.y, minAngle , maxAngle) , RotAmount.z);            
        
        if(Input.GetMouseButtonDown(0)){
            gunAnim.SetTrigger("Shoot");
            LockCursor();
            Ray ray = viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                if(hit.transform.tag == "Enemy"){
                    // EnemyScript.Instance.TakeDamage();
                    }
                }
        }
    
    }


    // Lock cursor
    public void LockCursor(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TakeDamage(int damageAmount){
        currentHealth -= damageAmount;
        if (currentHealth <= 0){
            deathScreen.SetActive(true);
            Time.timeScale=0;
        }
    }
}
