using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private CharacterController characterController;

 //   public float speed = 5.0f;

//    public float rotationspeed = 240.0f;

//   private float gravity = 20.0f;

//    private Vector3 moveDir = Vector3.zero;

    public HUD hud;

    public Inventory inventory;

    private InventoryItem itemPickUp = null;

    private bool mLockPickUp;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPickUp != null && Input.GetKeyDown(KeyCode.F)) {
            inventory.addItem(itemPickUp);
            itemPickUp.OnPickUp();
            itemPickUp = null;
            hud.CloseMsgPanel();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            // close Inventory here
        }

    /*    float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0)
            v = 0;

        transform.Rotate(0, h * rotationspeed * Time.deltaTime, 0);

        if (characterController.isGrounded)
        {
            bool move = (v > 0 || (h != 0));
            moveDir = Vector3.forward * v;
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;
        }
        moveDir.y -= gravity * Time.deltaTime;

        characterController.Move(moveDir * Time.deltaTime);
    */
    }

    private void OnTriggerEnter(Collider collider)
    {
         InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
         if (inventoryItem != null)
         {
             if (mLockPickUp)
                 return;

             itemPickUp = inventoryItem;
             hud.OpenMsgPanel("");
       }
       

    }
    private void OnTriggerExit(Collider collider)
    {
        InventoryItem inventoryItem = collider.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            hud.CloseMsgPanel();
            itemPickUp = null;
        }

    }
}
