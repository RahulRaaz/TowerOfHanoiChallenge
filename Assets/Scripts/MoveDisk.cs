using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MoveDisk : MonoBehaviour
{
    GameObject controlledDisc = null; //select disk to manipulate
    bool selected = false; //check if disk is actively selected
    List<GameObject> listOfDiscs = new List<GameObject>(5); // used to do operations on all disks
    public static int moveCount = 0;  //Game score calculation using scores
    public static float gameTime = 0.0f; //Time taken to complete game
    bool moved = false; //Check if object is moved - to add to move count
    Vector3 startVertPos = new Vector3(0, 0, 0); //used for vertical movement - start coordinates
    Vector3 endVertPos = new Vector3(0, 0, 0); //used for vertical movement - end coordinates
    Vector3 startHorzPos = new Vector3(0, 0, 0);//used for horizontal movement - start coordinates
    Vector3 endHorzPos = new Vector3(0, 0, 0); //used for horizontal movement - end coordinates
    bool movingVert = false; //to see if vertical movement is being done
    bool movingHorz = false; //to see if horizontal movement is being done
    GameObject mainCamera;
    float step;//identify step distance
    GameObject errMove;
    GameObject errPick;
    GameObject errDrop;
    // Start is called at beginning
    void Start()
    {
        //Add all discs to the list
        listOfDiscs.AddRange(new List<GameObject>() { GameObject.FindGameObjectsWithTag("Disk5")[0], GameObject.FindGameObjectsWithTag("Disk4")[0], GameObject.FindGameObjectsWithTag("Disk3")[0], GameObject.FindGameObjectsWithTag("Disk2")[0], GameObject.FindGameObjectsWithTag("Disk1")[0] });
        Time.timeScale = 1;
        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        step = 20.0f * Time.deltaTime;
        errPick = GameObject.FindGameObjectsWithTag("ErrorPick")[0];
        errPick.SetActive(false);
        errMove = GameObject.FindGameObjectsWithTag("ErrorMove")[0];
        errMove.SetActive(false);
        errDrop = GameObject.FindGameObjectsWithTag("ErrorDrop")[0];
        errDrop.SetActive(false);
    }

    // Update is called once per frame 
    void Update()
    {
        gameTime += Time.deltaTime; //update gametime
        //Select Disc and Move Vertically
        if (Time.timeScale == 0)
            return;
        if (Input.GetMouseButtonUp(0) && selected == false && !movingHorz && !movingVert)
        {
            //Identify object clicked on and select if it is a disc
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.collider.gameObject.layer == 3 && hit.transform != null)
            {
                controlledDisc = hit.transform.gameObject;
                if (controlledDisc.GetComponent<Rigidbody>().useGravity == true)
                    return;
                startVertPos = controlledDisc.transform.position;
                print("object selected " + controlledDisc.tag);
                bool isTop = checkTopDisc(controlledDisc, startVertPos); //check if the disc selected is the top disc on that rod
                if (isTop)
                {
                    print("Select approved for " + controlledDisc.tag);
                    endVertPos = new Vector3(startVertPos.x, 12.0f, 0); //set end position above rods
                    selected = true;
                    movingVert = true;
                }
                else
                {
                    print("Unable to select Disk, Other Disks on top. Try again. " + controlledDisc.tag);
                    controlledDisc = null;
                    errPick.SetActive(true);
                    return;
                }
            }
        }
        //Move Selected Disc Down and deselect
        else if (selected == true && Input.GetMouseButtonUp(0) && !movingHorz && !movingVert)
        {
            print("Disc drop detected " + controlledDisc.tag);
            if (checkSmallerDisc(controlledDisc, controlledDisc.transform.position)) //check if the selected disc is not placed above a smaller disc before drop
            {
                print("Proceed to disc drop " + controlledDisc.tag);
                toggleGravity(controlledDisc, true); //enable gravity for the disk to move it
                selected = false;
                if (moved)
                {
                    moveCount++;
                    moved = false;
                }
            }
            else
            {
                //Show error on screen to select another column
                errDrop.SetActive(true);
                print("Unable to drop here " + controlledDisc.tag);
            }
        }
        //Move Selected Disc Horizontally
        if (selected && !movingVert && !movingHorz)
        {
            startHorzPos = controlledDisc.transform.position;
            //Move disc to left if allowed
            if (Input.GetKeyUp("left"))
            {
                endHorzPos = controlledDisc.transform.position;
                print("Move left detected");
                if (startHorzPos.x >= 0 && startHorzPos.x <= 12)
                {
                    //Update position to next column
                    endHorzPos = new Vector3(startHorzPos.x - 12.0f, startHorzPos.y, 0.0f);
                    moved = true;
                    movingHorz = true;
                }
                if (startHorzPos.x == -12)
                {
                    //Print Error to console
                    print("Unable to move in direction");
                    errMove.SetActive(true);
                }
            }
            //Move disc to right if allowed
            else if (Input.GetKeyUp("right"))
            {
                endHorzPos = controlledDisc.transform.position;
                print("Move right detected");
                if (startHorzPos.x <= 0 && startHorzPos.x >= -12)
                {
                    //Update position to next column
                    endHorzPos = new Vector3(startHorzPos.x + 12.0f, startHorzPos.y, 0.0f);
                    moved = true;
                    movingHorz = true;
                }
                if (startHorzPos.x == 12)
                {
                    //Print Error to console
                    print("Unable to move in direction");
                    errMove.SetActive(true);
                }
            }
            //check if position has to be changed and do the movement
        }
        moveSmooth(); //Start movement of the discs vertically or horizontally
        if (!selected)
            resetCam();
        else
            selectCam();
        checkGameCompleted();
    }

    //Move the object smoothly in horizontal and vertical axes
    void moveSmooth()
    {
        float moveSpeed = 20.0f;//standard speed of disk movement
        float step = moveSpeed * Time.deltaTime; //identify step distance
        if (startVertPos != endVertPos && endVertPos.y!= 0.0f)
        {
            print("inside vertMove");
            print(endVertPos);
            controlledDisc.transform.position = Vector3.MoveTowards(startVertPos, endVertPos, step); //move vertically in steps
            startVertPos = controlledDisc.transform.position;
            //SetCameraMove here
            if (startVertPos.y == endVertPos.y)
            {
                movingVert = false;
                endVertPos = new Vector3(0, 0, 0);
            }

        }
        else if (startHorzPos != endHorzPos && endHorzPos.y!=0.0f)
        {
            Vector3 camPos = mainCamera.transform.position;
            print("inside horizMove");
            controlledDisc.transform.position = Vector3.MoveTowards(startHorzPos, endHorzPos, step); //move horizontally in steps
            startHorzPos = controlledDisc.transform.position;
            if (startHorzPos.x == endHorzPos.x)
            {
                movingHorz = false;
                endHorzPos = new Vector3(0, 0, 0);
            }
            if (camPos.x != startHorzPos.x)
            {
                camPos = new Vector3(startHorzPos.x / 2.0f, camPos.y, camPos.z);
                mainCamera.transform.position = camPos;
            }
            print("cam move complete");
        }
    }

    //Check if disc to be moved is the top disk for the rod 
    bool checkTopDisc(GameObject refDisc, Vector3 refPos)
    {
        print("inside checkTopDisk" + refDisc.tag);
        bool isTop = true;
        for (int i = 0; i < listOfDiscs.Count; i++)
            if (refDisc != listOfDiscs[i])
            {
                Vector3 posDisc = listOfDiscs[i].transform.position;
                if (refPos.x == posDisc.x && refPos.y < posDisc.y) //compare y coordinates of disks in same x coordinate
                    isTop = false;
            }
        return isTop;
    }

    //check if the disc selected disc is smaller than discs below it
    bool checkSmallerDisc(GameObject refDisc, Vector3 refPos)
    {
        print("inside checkSmallerDisc");
        Vector3 sizeOfReference = checkSizeDisc(refDisc);
        List<GameObject> discSameRod = new List<GameObject>(5);
        foreach (GameObject disc in listOfDiscs)
            if (refPos.x == disc.transform.position.x && disc != refDisc) //get all disks on same x coordinates
                discSameRod.Add(disc); 
        if (discSameRod.Count >= 1)
        {
            GameObject topDisc = findTopDisk(discSameRod);
            Vector3 topDiskSize = checkSizeDisc(topDisc);
            if (topDiskSize.x > sizeOfReference.x) //compare sizes of objects
                return true;
            else
                return false;
        }
        else
            return true;
    }

    //find top disk on selected rod (x axis)
    GameObject findTopDisk(List<GameObject> sameRodDisk)
    {
        print("inside findTopDisk");
        //Check y coordinates of the objects here and return object with highest Y
        GameObject highest = sameRodDisk[0];
        if (sameRodDisk.Count > 1)
        {
            for (int i = 1; i < sameRodDisk.Count; i++)
            {
                if (highest.transform.position.y < sameRodDisk[i].transform.position.y) //compare y axis coordinates of objects in same x axis
                    highest = sameRodDisk[i];
            }
        }
        return highest;
    }

    //check size of dics for comparison
    Vector3 checkSizeDisc(GameObject discToCheck)
    {
        print("inside checkSizeDisc");
        //Find and return the size of the object to compare
        Collider objCollider = discToCheck.GetComponent<Collider>();
        Vector3 size = objCollider.bounds.size;
        return size;
    }

    //enable gravity and disable kinematics for object or vice-versa
    void toggleGravity(GameObject disc, bool grav)
    {
        disc.GetComponent<Rigidbody>().useGravity = grav; //toggle gravity
        disc.GetComponent<Rigidbody>().isKinematic = !grav; //toggle kinematics
    }

    //check if game is completed
    void checkGameCompleted()
    {
        List<float> xPos = new List<float>(5);
        for (int i = 0; i < listOfDiscs.Count; i++)
            xPos.Add(listOfDiscs[i].transform.position.x); //get horizontal position of all disks
        if (xPos[0] == xPos[1] && xPos[0] == xPos[2] && xPos[0] == xPos[3] && xPos[0] == xPos[4] && xPos[4] !=-12 && !selected)
        {
            if (!controlledDisc.GetComponent<Rigidbody>().useGravity)
            {
                StartCoroutine("wait");
                SceneManager.LoadScene(2);
            }
        }
    }

    //reset cam to default position on dropping disc
    void resetCam()
    {
        print("ResetCam Started");
        Vector3 camStartPos = mainCamera.transform.position;
        Vector3 camEndPos = new Vector3(0, camStartPos.y, camStartPos.z);
        if (camStartPos.x != 0)
            mainCamera.transform.position = Vector3.MoveTowards(camStartPos, camEndPos, step / 4);
    }

    //move cam closer to selected disc
    void selectCam()
    {
        print("moving cam to selection");
        Vector3 camStartPos = mainCamera.transform.position;
        Vector3 camEndPos = new Vector3(controlledDisc.transform.position.x/2, camStartPos.y, camStartPos.z);
        if (camStartPos.x != camEndPos.x)
            mainCamera.transform.position = Vector3.MoveTowards(camStartPos, camEndPos, step / 2);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
    }
}