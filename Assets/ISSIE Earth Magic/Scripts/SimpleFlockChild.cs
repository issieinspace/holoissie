using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlockChild : MonoBehaviour
{

    public Transform _thisT; //Reference to the transform component
    public GameObject _model; // Reference to bird model

    public Transform _modelT; // Reference to bird model transform (caching tranform to avoid any extra getComponent calls)
    public float LifetimeSeconds = 5;
    public float MinSpeed = 0.1f;
    public float MaxSpeed = 0.5f;
    private float _lifetimeCounter = 0.0f;
    public string FlapAnimation;
    public string SoarAnimation;

    public int _updateDivisor = 1;
    public int _updateCounter;

    private int _updateNextSeed = 0;
    private float _speed;
    public float _damping; //Damping used for steering (steer speed)
    private int _updateSeed = -1;
    private bool _instantiated = false;
    private bool _soar = false;
    private float _soarTimer = 0.0f;
    private float _newDelta = 0.1f;
    private float _targetSpeed = 0f;
    private bool _move = true;
    private Vector3 _wayPoint;

    public void Start()
    {
        FindRequiredComponents(); //Check if references to transform and model are set (These should be set in the prefab to avoid doind this once a bird is spawned, click "Fill" button in prefab)
        Wander(0.0f);
        SetRandomScale();
        //_thisT.position = findWaypoint();
        _thisT.position = Camera.main.transform.position + Camera.main.transform.forward * -2.0f +
                          Camera.main.transform.right * (Random.value - 1.0f) * 2.0f +
                          Camera.main.transform.up * (Random.value) * 2.0f;
        RandomizeStartAnimationFrame();
        //InitAvoidanceValues();
        _instantiated = true;
        if (_updateDivisor > 1)
        {
            int _updateSeedCap = _updateDivisor - 1;
            _updateNextSeed++;
            this._updateSeed = _updateNextSeed;
            _updateNextSeed = _updateNextSeed % _updateSeedCap;
        }
    }

    public void Update()
    {
        //Skip frames
        if (_updateDivisor <= 1 || _updateCounter == _updateSeed)
        {
            SoarTimeLimit();
            //CheckForDistanceToWaypoint();
            RotationBasedOnWaypointOrAvoidance();
            LimitRotationOfModel();
            LifetimeCheck();
        }
    }

    void LifetimeCheck()
    {
        if (_lifetimeCounter < LifetimeSeconds)
        {
            _lifetimeCounter += 0.01f;
        }
        else
        {
            Destroy(_model);
            Destroy(this);
        }
    }
    

    public void OnDisable()
    {
        CancelInvoke();
    }

    public void OnEnable()
    {
        if (_instantiated)
        {
            _model.GetComponent<Animation>().Play(FlapAnimation);
        }
    }

    public void FindRequiredComponents()
    {
        if (_thisT == null) _thisT = transform;
        if (_model == null) _model = _thisT.FindChild("Model").gameObject;
        if (_modelT == null) _modelT = _model.transform;
    }

    public void RandomizeStartAnimationFrame()
    {
        foreach (AnimationState state in _model.GetComponent<Animation>())
        {
            state.time = Random.value * state.length;
        }
    }

    public void RotationBasedOnWaypointOrAvoidance()
    {

        Vector3 lookit = _wayPoint - _thisT.position;
        if (_targetSpeed > -1 && lookit != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookit);

            //_thisT.rotation = Quaternion.Slerp(_thisT.rotation, rotation, _newDelta * _damping);
        }

        _speed = Mathf.Lerp(_speed, _targetSpeed, _newDelta * 2.5f);
        //Position forward based on object rotation
        if (_move)
        {
            _thisT.position += _thisT.forward * _speed * _newDelta;
        }
    }
    public void SetRandomScale()
    {
        float sc = Random.Range(0.7f, 1f);
        _thisT.localScale = new Vector3(sc, sc, sc);
    }

    //Soar Timeout - Limits how long a bird can soar
    public void SoarTimeLimit()
    {
        if (this._soar)
        {
            if (_soarTimer > 3)
            {
                this.Flap();
                _soarTimer = 0.0f;
            }
            else
            {
                _soarTimer += _newDelta;
            }
        }
    }


    public void LimitRotationOfModel()
    {
        Quaternion rot = Quaternion.identity;
        Vector3 rotE = Vector3.zero;
        rot = _modelT.localRotation;
        rotE = rot.eulerAngles;
        if (_soar)
        {
            rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, -_thisT.localEulerAngles.x,
                _newDelta * 1.75f);
            rot.eulerAngles = rotE;
            _modelT.localRotation = rot;
        }
        else
        {
            rotE.x = Mathf.LerpAngle(_modelT.localEulerAngles.x, 0.0f, _newDelta * 1.75f);
            rot.eulerAngles = rotE;
            _modelT.localRotation = rot;
        }
    }

    public void Wander(float delay)
    {
        _damping = Random.Range(1, 2);
        _targetSpeed = Random.Range(MinSpeed, MaxSpeed);
        Invoke("SetRandomMode", delay);
    }

    public void SetRandomMode()
    {
        CancelInvoke("SetRandomMode");
        if (Random.value < 0.7f)
        {
            Soar();
        }
        else
        {
            Flap();
        }
    }

    public void Flap()
    {
        if (_move)
        {
            if (this._model != null) _model.GetComponent<Animation>().CrossFade(FlapAnimation, .5f);
            _soar = false;
            animationSpeed();
            _wayPoint = findWaypoint();
        }
    }

    public Vector3 findWaypoint()
    {
        Vector3 t = Vector3.zero;
        t.x = Random.Range(-4f, 4f) + Camera.main.transform.forward.x;
        t.y = Random.Range(-4f, 4f) + Camera.main.transform.forward.y;
        t.z = Random.Range(-1f, 1f) + Camera.main.transform.forward.z;
        //t.x = Random.Range(-_spawnSphere, _spawnSphere) + _posBuffer.x;
        //t.z = Random.Range(-_spawnSphereDepth, _spawnSphereDepth) + _posBuffer.z;
        //t.y = Random.Range(-_spawnSphereHeight, _spawnSphereHeight) + _posBuffer.y;
        return t;
    }

    public void Soar()
    {
        if (_move)
        {
            _model.GetComponent<Animation>().CrossFade(SoarAnimation, 1.5f);
            _wayPoint = findWaypoint();
            _soar = true;
        }
    }


    public void animationSpeed()
    {
        foreach (AnimationState state in _model.GetComponent<Animation>())
        {
            state.speed = Random.Range(2, 4);
        }
    }
}

