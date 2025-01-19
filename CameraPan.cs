using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private bool canMove = true;
    public float panSpeed = 10;
    public float panBorderThickness = 100;


    // Update is called once per frame
    void Update()
    {

        // Toggle movement
        if (Input.GetKey(KeyCode.Escape))
        {
            canMove = !canMove;
        }

        // Exit early if movement is toggled off
        if (!canMove)
        {
            return;
        }

        // Exit early if mouse is out of vertical bounds
        if (Input.mousePosition.y > Screen.height || Input.mousePosition.y < 0)
        {
            return;
        }

        // Exit early if mouse is out of horizontal bounds
        else if (Input.mousePosition.x > Screen.width || Input.mousePosition.x < 0)
        {
            return;
        }

        // Forward pan
        if (Input.mousePosition.y >= Screen.height - panBorderThickness || Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        // Backwards pan
        else if (Input.mousePosition.y <= panBorderThickness || Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        // Right pan
        if (Input.mousePosition.x >= Screen.width  - panBorderThickness || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        // Left pan
        else if (Input.mousePosition.x <= panBorderThickness || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.X))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime);
        }

    }
}
