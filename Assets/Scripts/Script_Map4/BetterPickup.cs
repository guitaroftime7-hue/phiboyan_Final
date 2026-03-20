using UnityEngine;

public class BetterPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform holdPoint;
    public float throwForce = 12f;
    public float holdSmooth = 15f;

    GameObject heldObject;
    Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
        }

        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            ThrowObject();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null)
        {
            heldRb.MovePosition(Vector3.Lerp(
                heldObject.transform.position,
                holdPoint.position,
                Time.fixedDeltaTime * holdSmooth));
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody>();

                heldRb.useGravity = false;
                heldRb.linearDamping = 10;
            }
        }
    }

    void ThrowObject()
    {
        heldRb.useGravity = true;
        heldRb.linearDamping = 0;

        heldRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
    }
}