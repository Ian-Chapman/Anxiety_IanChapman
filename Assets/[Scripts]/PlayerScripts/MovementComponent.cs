using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovementComponent : MonoBehaviour
{
    public Animator leftWallAnimator;
    public Animator rightWallAnimator;
    public Animator frontWallAnimator;
    public Animator backWallAnimator;

    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;

    //components
    private PlayerController playerController;
    Rigidbody rigidbody;
    Animator playerAnimator;
    public GameObject followTarget;

    //references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    public float aimSensitivity = 1;

    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");


    //GameObjects the player will collect:
    [Header("Collectable Objects")]
    public GameObject record;
    public GameObject painting;
    public GameObject toyCar;
    public GameObject teddyBear;
    public GameObject computerKeyboard;
    public GameObject toyElephant;
    public GameObject coffeeMug;
    public GameObject castleBlock;
    public GameObject storyBook;
    public GameObject comicBook;
    public GameObject clock;
    public GameObject toyBoat;
    public GameObject lavaLamp;

    [Header ("Text Objects")]
    public GameObject recordText;
    public GameObject paintingText;
    public GameObject toyCarText;
    public GameObject teddyBearText;
    public GameObject computerKeyboardText;
    public GameObject toyElephantText;
    public GameObject coffeeMugText;
    public GameObject castleBlockText;
    public GameObject storyBookText;
    public GameObject comicBookText;
    public GameObject clockText;
    public GameObject toyBoatText;
    public GameObject lavaLampText;


    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        recordText.SetActive(true);
        paintingText.SetActive(true);
        toyCarText.SetActive(true);
        teddyBearText.SetActive(true);
        computerKeyboardText.SetActive(true);
        toyElephantText.SetActive(true);
        coffeeMugText.SetActive(true);
        castleBlockText.SetActive(true);
        storyBookText.SetActive(true);
        comicBookText.SetActive(true);
        clockText.SetActive(true);
        toyBoatText.SetActive(true);
        lavaLampText.SetActive(true);
    }

    void Update()
    {
        ///horizontal rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        //verical rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.right);

        //Clap angl
        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.transform.localEulerAngles.x;

        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }

        followTarget.transform.localEulerAngles = angles;

        //rotate the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        if (!(inputVector.magnitude > 0))
        {
            moveDirection = Vector3.zero;
        }

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;
        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);

        transform.position += movementDirection;



        OnPlayerWin();


    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);

    }

    public void OnJump(InputValue value)
    {
        if (playerController.isJumping)
        {
            return;
        }

        playerController.isJumping = value.isPressed;
        rigidbody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }


    //Lose Condition
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "BackWall") && (other.gameObject.tag == "LeftWall") &&
            (other.gameObject.tag == "RightWall") && (other.gameObject.tag == "FrontWall"))
        {
            //On Player Lose
            StartCoroutine(RunLoseSequence());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") && !playerController.isJumping)
        {
            return;
        }

        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, false);

        //Destroy the collectable objects and remove their text from the canvas
        if (other.gameObject.tag == "Record") 
            { Destroy(other.gameObject); 
            recordText.SetActive(false); }
        
        if (other.gameObject.tag == "Painting") 
            { Destroy(other.gameObject);
            paintingText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Toy Car") 
            { Destroy(other.gameObject);
            toyCarText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Teddy Bear") 
            { Destroy(other.gameObject);
            teddyBearText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Computer Keyboard") 
            { Destroy(other.gameObject);
            computerKeyboardText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Toy Elephant") 
            { Destroy(other.gameObject);
            toyElephantText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Coffee Mug") 
            { Destroy(other.gameObject);
            coffeeMugText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Castle Block") 
            { Destroy(other.gameObject);
            castleBlockText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Story Book") 
            { Destroy(other.gameObject);
            storyBookText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Comic Book") 
            { Destroy(other.gameObject);
            comicBookText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Clock") 
            { Destroy(other.gameObject);
            clockText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Toy Boat") 
            { Destroy(other.gameObject);
            toyBoatText.SetActive(false);
        }
        
        if (other.gameObject.tag == "Lava Lamp") 
            { Destroy(other.gameObject);
            lavaLampText.SetActive(false);
        }
    }

    public void OnPlayerWin()
    {
        if ((record.gameObject == null) && 
                (painting.gameObject == null) &&
                (toyCar.gameObject == null) &&
                (teddyBear.gameObject == null) &&
                (computerKeyboard.gameObject == null) &&
                (toyElephant.gameObject == null) &&
                (coffeeMug.gameObject == null) &&
                (castleBlock.gameObject == null) &&
                (storyBook.gameObject == null) &&
                (comicBook.gameObject == null) &&
                (clock.gameObject == null) &&
                (toyBoat.gameObject == null) &&
                (lavaLamp.gameObject == null))
        {
            StartCoroutine(RunWinSequence());
        }
    }


    public IEnumerator RunWinSequence()
    {
        backWallAnimator.SetBool("isGameComplete", true);
        frontWallAnimator.SetBool("isGameComplete", true);
        leftWallAnimator.SetBool("isGameComplete", true);
        rightWallAnimator.SetBool("isGameComplete", true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("WinScene");
    }

    public IEnumerator RunLoseSequence()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("LoseScene");
    }

}
