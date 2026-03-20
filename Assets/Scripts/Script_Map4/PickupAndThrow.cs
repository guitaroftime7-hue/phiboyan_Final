using UnityEngine;

public class PickupAndThrow : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform handPoint;
    public float throwForce = 10f;

    public GameObject crosshair;

    private GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject != null)
                ThrowObject();
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

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

                heldObject.transform.SetParent(handPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                if (crosshair != null)
                    crosshair.SetActive(true);
            }
        }
    }

    void ThrowObject()
    {
        GameObject obj = heldObject;
        heldObject = null;

        obj.transform.SetParent(null);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        if (crosshair != null)
            crosshair.SetActive(false);
    }
}