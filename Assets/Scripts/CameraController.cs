using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour {
 
    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun= 1.0f;
     
    void Update () {

        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey (KeyCode.LeftShift)){
            totalRun += Time.deltaTime;
            p  = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else{
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }
       
        p = p * Time.deltaTime;
        float curZoomPos;
        float zoom= 0f; 
        Vector3 zoom_change;
        

        // Attaches the float y to scrollwheel up or down
        float mouse_scroll = Input.mouseScrollDelta.y;
        Debug.Log (mouse_scroll);
        // If the wheel goes up it, decrement 5 from "zoomTo"
        if (mouse_scroll >= 1)
        {
            zoom -= 1f;
        }
 
        // If the wheel goes down, increment 5 to "zoomTo"
        else if (mouse_scroll <= -1) {
            zoom += 1f;
        }

        zoom = Mathf.Clamp (zoom, -5f, 5f);
        zoom_change = new Vector3(0, zoom, 0);
        //transform.localPosition += zoom_change * Time.deltaTime;
        transform.localPosition += zoom_change;

        //update sence wsad direction
        Vector3 newPosition = transform.position;
        transform.Translate(p);
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }   

    //move camera postion according to mouse on edge
    //returns the basic values, if it's 0 than it's not active.
    private Vector3 GetBaseInput() { 
        Vector3 p_Velocity = new Vector3();
        if ( Input.mousePosition.y >= Screen.height *0.95) 
         {
            p_Velocity += new Vector3(0, 1, 0);
         }
        if ( Input.mousePosition.y <= Screen.height *0.05)
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if ( Input.mousePosition.x <= Screen.width *0.05)
         {
            p_Velocity += new Vector3(-1, 0, 0);
         }
        if ( Input.mousePosition.x >= Screen.width *0.95)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
    
}