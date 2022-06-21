using UnityEngine.EventSystems;
using UnityEngine;

 [RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;
    public LayerMask movementMask;
    Camera cam;
    PlayerMotor motor;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if player is hovering over UI, so player won't move when clicking on UI elements.
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //Player can hold right click to move continously towards cursor
        if( Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Move our player towards what we hit
                motor.MoveToPoint(hit.point);
            }

        //Won't focus if player is holding right click and hovers over interactable. Player must dedicate click to selecting interactable.
            if(Input.GetMouseButtonDown(1))
            {
                if(Physics.Raycast(ray, out hit, 100))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    // Stop focusing any objects
                    if(interactable != null)
                    {
                        SetFocusFollow(interactable);
                        
                    }
                    else 
                    {
                        motor.StopFollowingTarget();
                    }
                }
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

        //Check if we click an object and set it as focus.
            if(Physics.Raycast(ray, out hit, 100))
            {
                
                
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                
                if(interactable != null)
                {
                    SetFocus(interactable);
                }
                else
                {
                    RemoveFocus();
                }
            }
        }
    }
    void SetFocus (Interactable newFocus)
    {
        if (newFocus != focus)
        {
            focus = newFocus;
            focus.OnDefocused();
        }

        // newFocus.OnFocused(transform);
    }

    void SetFocusFollow (Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}
