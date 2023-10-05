using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    float ballRadius;
    float ballDiameter;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;


    // Start is called before the first frame update
    void Start()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius * 100f;
        ballDiameter = ballRadius * 2f;
        PlaceAllBalls();
    }

    // Update is called once per frame
    void PlaceAllBalls()
    {
        PlaceCueBall();
        PlaceRandomBalls();
    }

    void PlaceCueBall()
    {
        GameObject ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }
    void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeEightBall();
    }

    void  PlaceRandomBalls()
    {
        int NumInThisRow = 1;
        int rand;
        Vector3 firstInRowPosition = headBallPosition.position;
        Vector3 currentPosition = firstInRowPosition;

        void PlaceRedBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion .identity);
            ball.GetComponent<Ball>().BallSetup(true);
            redBallsRemaining--;
        }

        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(false);
            blueBallsRemaining--;
        }
        // oUTER LOOP iS tHE 5 rOWS
        for(int i = 0; i < 5; i++) 
        {
            // iNNER Loops are the balls in each row
            for (int j = 0; j < NumInThisRow; j++)
            { 
                // Check to see if it's the middle spot where the 8 ball goes
                if (i== 2 && j== 1)
                {
                    PlaceEightBall(currentPosition);
                }
                // If THERE ARE RED AND BLUE BALLS REM., RANDOMLY CHOOSE ONE AND PLACE IT
                else if (redBallsRemaining>0 && blueBallsRemaining>0)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        PlaceRedBall(currentPosition);
                    }
                    else
                    {
                        PlaceBlueBall(currentPosition);
                    }
                }
                // IF THHERE ARE ONLY RED BALLS REMAINING, PLACE ONNE
                else if (redBallsRemaining>0)
                {
                    PlaceRedBall(currentPosition);
                }
                // OTHERWISE PLACE A BLUE ONE
                else
                {
                    PlaceBlueBall(currentPosition);
                }
                // MOVE THE CURRENT POSITION FOR THE NEXT BALL IN THIS ROW TO THE RIGHT
                currentPosition += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            // MOVE TO THE NEXT ROW
            firstInRowPosition += Vector3.back * (Mathf.Sqrt(3) * ballRadius) + Vector3.left * ballRadius;
            currentPosition = firstInRowPosition;
            NumInThisRow++;
        }
    }
}
