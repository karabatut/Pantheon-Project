using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    MovementEnum movementState; //Player's movement state
    int platformLength = 5; //Total platform length
    int platformSize = 27;  //One platform length
    public GameObject platformObject;
    public GameObject playerObject;
    public GameObject opponentObject;
    public GameObject staticObstacleObject;
    public GameObject horizontalObstacleObject;
    public GameObject endWallObject;
    List<GameObject> runners;
    List<GameObject> finishers;
    GameObject finishPlatform;
    GameObject endWall;
    DrawManager drawManager;
    bool finished; //if the player finished the race.

    // Start is called before the first frame update
    void Start()
    {
        runners = new List<GameObject>();
        finishers = new List<GameObject>();
        movementState = MovementEnum.FORWARD; //Player's default movement state is forward
        createPlatform();
        createEndWall();
        createObstacles();
        createPlayer();
        createOpponent();
        finished = false; 
    }

    // Update is called once per frame
    void Update()
    {
        checkFinishers();
        updateRankings();
    }

    //Sets the movement state of the player
    public void setMovementState(MovementEnum newState)
    {
        movementState = newState;
    }

    //Returns the movement state of the player
    public MovementEnum getMovementState()
    {
        return movementState;
    }

    //Creates the platform using platformObject prefab. Creates a platform with a length of platformLength + 2. "+2" is the start and finish straights.
    void createPlatform()
    {
        Vector3 platformPosition = new Vector3(platformSize, 0f, 0f);

        //Creates platforms as many as the platform length
        //offsets each platform by 27 
        for(int i = 0; i < platformLength + 2; i++)
        {
            finishPlatform = Instantiate(platformObject, platformPosition, Quaternion.identity);
            platformPosition.x = platformPosition.x - platformSize;
        }
    }

    //Creates the player and gives it the name "You"
    void createPlayer()
    {
        Vector3 playerPosition = new Vector3(15f, 0.15f, 0f);
        GameObject player = Instantiate(playerObject, playerPosition, Quaternion.identity);
        player.transform.Rotate(new Vector3(0f, -90f, 0f));
        player.name = "You";
        runners.Add(player);
    }

    //Creates the opponents, gives them names from the OpponentNames class and orders them as a grid.
    //  * P *
    //  * * *
    //  * * *   ---> the order of the grid. The player is shown as P.
    //  *   *
    void createOpponent()
    {
        OpponentNames opponentNames = new OpponentNames();
        string[] names = opponentNames.getOpponentNames();
        float zPosition = -4.5f;
        float xPosition = 15f;
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            if(count == 4)
            {
                if(zPosition == -4.5f)
                {
                    xPosition = 15f;
                    zPosition = 4.5f;
                    count = 0;
                }
                else
                {
                    xPosition = 18f;
                    zPosition = 0f;
                }
                
            }
            Vector3 opponentPosition = new Vector3(xPosition, 0.15f, zPosition);
            GameObject opponent = Instantiate(opponentObject, opponentPosition, Quaternion.identity);
            opponent.transform.Rotate(new Vector3(0f, -90f, 0f));
            opponent.name = names[i];
            xPosition = xPosition + 3;
            count++;
            runners.Add(opponent);
        }

        
    }

    //Creates obstacles at random.
    void createObstacles()
    {
        
        for (int i = 0; i < (platformLength * platformSize) / 8; i++)
        {
            float zPosition = Random.Range(-6f, 6f);
            Vector3 obstaclePos = new Vector3((i * 7f * -1), 1.5f, zPosition);
            if (Random.Range(0, 2) == 1) //50% possibility of a static object or moving object.
            {
                GameObject staticObstacle = Instantiate(staticObstacleObject, obstaclePos, Quaternion.identity);
                staticObstacle.transform.Rotate(new Vector3(0f, 90f, 0f));
                staticObstacle.tag = "StaticObstacle";
            }
            else
            {
                obstaclePos = obstaclePos + new Vector3(0f, 2.2f, 0f);
                GameObject oorizontalObstacle = Instantiate(horizontalObstacleObject, obstaclePos, Quaternion.identity);
                oorizontalObstacle.tag = "HorizontalObstacle";
            }
            
            
        }
        

    }

    //Creates the end wall that the player has to paint.
    void createEndWall()
    {
        Vector3 endWallPosition = new Vector3(platformSize * (platformLength + 1) * -1, 5f, 0f);
        endWall = Instantiate(endWallObject, endWallPosition, Quaternion.identity);
        endWall.transform.Rotate(new Vector3(0f, 0f, -90f));
        drawManager = (DrawManager)endWall.GetComponent(typeof(DrawManager));
    }

    //Sorts all of the runners based on their position on the X-Axis.
    void updateRankings()
    {
        runners.Sort((p, q) => p.transform.position.x.CompareTo(q.transform.position.x));
    }

    //Returns sorted runners.
    public List<GameObject> getRankings()
    {
        return runners;
    }

    //Returns finished runners.
    public List<GameObject> getFinishers()
    {
        return finishers;
    }

    //Checks which runners have finished the race.
    void checkFinishers()
    {
        foreach (GameObject runner in runners)
        {
            //If a runner crosses the finish platforms position, it finishes the race.
            if(runner.transform.position.x < finishPlatform.transform.position.x)
            {
                if(runner.tag == "Player")
                {
                    runner.GetComponent<PlayerController>().setMovementState(MovementEnum.FINISHED);
                    finished = true;
                }
                else
                {
                    runner.GetComponent<OpponentController>().setMovementState(MovementEnum.FINISHED);
                }

                //if a new runner finishes the race, add it to the finishers list.
                if (!finishers.Contains(runner))
                {
                    finishers.Add(runner);
                }
                
            }
        }
    }

    //Return if the player finished the race.
    public bool isFinished()
    {
        return finished;
    }

    //Return the painted percentage of the end wall.
    public float getPaintedPercentage()
    {
        return drawManager.getPercentage();
    }
}
