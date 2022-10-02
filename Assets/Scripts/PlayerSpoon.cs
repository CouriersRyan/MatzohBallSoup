using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerSpoon : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private float spinSpd = 1f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform line;
    [SerializeField] private Vector2 lineY;
    [SerializeField] private DeformableMesh matzohBall;
    [SerializeField] private BoundsChecker bound;
    private float _time = 0;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text scoreKeeper;
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private TMP_Text displayText;

    private int _score;
    private int _highScore;
    private int _iterations;
    
    private float _lineTime;
    private bool _started = false;
    private bool _eating;
    private bool _finished;
    private EatState _eatState = EatState.Idle;
    

    //While held down, spins around the bowl. Can't be held again when released.
    private bool _spin;
    void OnSpin(InputValue value)
    {
        _spin = value.Get<float>() > 0.5f;
    }

    //Moves in a line while held down back and forth.
    private bool _line;
    void OnLine(InputValue value)
    {
        _line = value.Get<float>() > 0.5f;
        
    }

    //Rotates the spoon while held down.
    private bool _vector;
    void OnVector(InputValue value)
    {
        _vector = value.Get<float>() > 0.5f;
    }

    // Lifts the spoon up.
    private bool _eat;
    void OnEat(InputValue value)
    {
        _eat = value.Get<float>() > 0.5f;
    }

    private void FixedUpdate()
    {
        if (_time <= 0)
        {
            displayPanel.SetActive(true);
            if (_iterations > 0)
            {
                if (_score > _highScore)
                {
                    _highScore = _score;
                }

                displayText.text = "Game Over!\nPress all four keys to start again.\nScore: " + _score +
                                   "\nHigh Score: " + _highScore + "\nTries: " + _iterations;
            }
            if(_spin && _line && _vector && _eat)
            {
                //Start game.
                matzohBall.Reset();
                _time = 100f;
                _score = 0;
                scoreKeeper.text = _score.ToString();
                displayPanel.SetActive(false);
                _iterations++;
            }
        }

        if (_time > 0)
        {
            if (_spin && _line && _vector && _eat && !_eating)
            {
                _started = true;
            }

            if (_spin && _started)
            {
                center.Rotate(Vector3.up, spinSpd);
            }

            if (_line && _started)
            {
                _lineTime += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(pointA.localPosition, pointB.localPosition,
                    Mathf.Sin(_lineTime) * 0.5f + 0.5f);
            }

            if (_vector && _started)
            {
                transform.Rotate(Vector3.up, spinSpd * 4f);
            }

            if (!_eat && _started)
            {
                _started = false;
                _eating = true;
                _eatState = EatState.Down;
            }

            if (_eating)
            {
                switch (_eatState)
                {
                    case EatState.Down:
                        if (line.localPosition.y > lineY.y)
                        {
                            line.Translate(Vector3.up * 0.1f);
                        }
                        else
                        {
                            line.localPosition = new Vector3(line.localPosition.x, lineY.y, line.localPosition.z);
                            _eatState = EatState.CheckBalance;
                        }

                        break;

                    case EatState.CheckBalance:
                        _eatState = bound.isMatzohOutOfBounds ? EatState.Lift : EatState.Scoop;
                        break;

                    case EatState.Scoop:
                        _finished = matzohBall.Eat();
                        _score++;
                        scoreKeeper.text = _score.ToString();
                        _eatState = EatState.MoveOut;
                        break;
                    case EatState.MoveOut:
                        if (transform.localRotation.eulerAngles.x < 90)
                        {
                            transform.Rotate(1, 0, 0, Space.Self);
                        }
                        else
                        {
                            _eatState = EatState.Lift;
                            Debug.Log("Done Rotating: " + transform.rotation.eulerAngles);
                        }

                        break;

                    case EatState.Lift:
                        if (line.localPosition.y < lineY.x)
                        {
                            line.Translate(Vector3.down * 0.1f);
                        }
                        else
                        {
                            line.localPosition = new Vector3(line.localPosition.x, lineY.x, line.localPosition.z);
                            _eatState = EatState.Reset;
                        }

                        break;

                    case EatState.Reset:
                        if (transform.localRotation.eulerAngles.x > 5)
                        {
                            transform.Rotate(-1, 0, 0, Space.Self);
                        }
                        else
                        {
                            transform.localRotation = Quaternion.Euler(0, 0, 0);
                            if (_finished)
                            {
                                matzohBall.Reset();
                                // Increase score by one.
                                _score+= 10;
                                scoreKeeper.text = _score.ToString();
                            }

                            _eatState = EatState.Idle;
                        }

                        break;

                    default:
                        _eating = false;
                        break;
                }
            }
            
            _time -= Time.deltaTime;
            timer.text = Mathf.RoundToInt(_time).ToString();
        }
    }

    private enum EatState
    {
        Down,
        CheckBalance,
        Scoop,
        MoveOut,
        Lift,
        Reset,
        Idle
    }
}
