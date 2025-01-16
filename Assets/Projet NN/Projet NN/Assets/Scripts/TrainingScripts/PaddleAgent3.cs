using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PaddleAgent3 : Agent
{
    float paddleY = -17f;
    float paddleX = 18f; 
    float moveSpeed = 50f;
    private float _xValue_to_left;
    private float _xValue_to_right;
    //float leftLimit = -33f;
    //float rightLimit = 33f;
    //float lastPositionDifference;
    private int rewardHitBrick;
    private int rewardHitBall;
    private int levelComplete;
    private int _prevBall = -1; 
    private int _currBall;
    private int _prevScore = 0; 
    private int _currScore;
    //private int brickCount = 0; 
    GameObject currentBall;
    //bool sameBallTrip; 
    //bool wallHit = false;
    private bool brickhit;


    [SerializeField] private Transform targetTransform;
    [SerializeField] private Rigidbody _ballRigidBody;
    [SerializeField] private Transform leftWallTransform;
    [SerializeField] private Transform rightWallTransform;
    //[SerializeField] private Transform ceilingTransform;

    
    public override void OnEpisodeBegin()
    {
        rewardHitBrick = 5;
        rewardHitBall = 5;
        levelComplete = 100;
        brickhit = false;
        //targetTransform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localPosition = new Vector3(paddleX, paddleY, 0f);
        _ballRigidBody.velocity = Vector3.down * moveSpeed;
    }

  
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        //Debug.Log(moveX);
        // paddle position + half wall width + half paddle width
        _xValue_to_left = transform.localPosition.x + (-1) * Time.deltaTime * moveSpeed + (-1 * leftWallTransform.localScale.x) + (-1 * transform.localScale.x / 2);
        _xValue_to_right = transform.localPosition.x + 1 * Time.deltaTime * moveSpeed + (-1 * leftWallTransform.localScale.x) + (-1 * transform.localScale.x / 2);

       
        if (moveX == 1)
        {
            
            if (_xValue_to_left < 0 + 0.5)
            {
                Debug.Log("Player 2: out of left boundary");
            }
            else if (_xValue_to_left <= 35 - 3.5 && _xValue_to_left >= 0 + 0.5)
            {
                transform.localPosition += new Vector3(-1 * Time.deltaTime * moveSpeed, 0, 0);
            }
        }
        else if (moveX == 2)
        {
          
            if (_xValue_to_right > 35 - 3.5)
            {
                Debug.Log("Player 2: out of right boundary");
            }
            else
            {
                transform.localPosition += new Vector3(1 * Time.deltaTime * moveSpeed, 0, 0);
            }
        }
        else
        {
            transform.localPosition += new Vector3(0 * Time.deltaTime * moveSpeed, 0, 0);
        }

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), transform.position.y, transform.position.z);
    }

  
    public override void CollectObservations(VectorSensor sensor)
    {
        
        currentBall = getCurrentBall();
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(leftWallTransform.localPosition.x);
        sensor.AddObservation(rightWallTransform.localPosition.x);
        if (currentBall != null)
        {
            sensor.AddObservation(currentBall.GetComponent<Rigidbody>().velocity);
            sensor.AddObservation(currentBall.transform.localPosition.x);
            sensor.AddObservation(currentBall.transform.localPosition.y);
            //observe the x value difference between ball and walls
            sensor.AddObservation(currentBall.transform.localPosition.x - leftWallTransform.localPosition.x);
            sensor.AddObservation(currentBall.transform.localPosition.x - rightWallTransform.localPosition.x);
        }
    }


    /*
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetAxisRaw("Horizontal");
    }*/



    private void OnCollisionEnter(Collision obj)
    {
        //Debug.Log("Collision: " + obj.gameObject.name);
        if (obj.gameObject == currentBall)
        {        
           
            if (brickhit == true)
            {
                brickhit = false;
                AddReward(rewardHitBall); 
                Debug.Log("Positive Reward - catch the ball");
            }
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("level: " + GameManager.Instance.levelCompleted);
        _currBall = GameManager.Instance.Balls2;
        _currScore = GameManager.Instance.Score2;

        
        if (_currBall < _prevBall && _currBall == 0)
        {
            EndEpisode();
            Debug.Log("Episode Num " + CompletedEpisodes);
        }

        
        if (_currScore > _prevScore)
        {
            Debug.Log("Positive Reward - hit a brick");
            AddReward(rewardHitBrick);
            Debug.Log("Got Reward: " + rewardHitBrick);
            brickhit = true;
        }

        
        if (GameManager.Instance.levelCompleted == true)
        {
            GameManager.Instance.levelCompleted = false;
            Debug.Log("Reward level completed : " + GameManager.Instance.levelCompleted);
            AddReward(levelComplete);
        }
        _prevBall = _currBall;
        _prevScore = _currScore;
    }

    
    private GameObject getCurrentBall()
    {
        currentBall = GameManager.Instance.currentBall2;
        return currentBall;
    }
}