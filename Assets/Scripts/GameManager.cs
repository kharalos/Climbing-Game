using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController camCont;
    [SerializeField] private Transform leftTarget, rightTarget;
    [SerializeField] private HandController leftHandC, rightHandC;
    [SerializeField] private GameObject playerHips;
    [SerializeField] private GameObject startScreen, endScreen;

    [SerializeField] private MeshRenderer leftEyeMR, rightEyeMR, hairMR;
    private Color blueEmissionCLR, boostEmissionCLR;

    private bool focusOnRight = true;

    private float boostTimer = 0;
    private bool canBoost = false;

    private bool gamestarted = false;
    private bool gameover = false;
    private void Start()
    {
        Time.timeScale = 0;
        blueEmissionCLR = leftEyeMR.material.GetColor("_EmissionColor");
        boostEmissionCLR = blueEmissionCLR;
        boostEmissionCLR = new Color(1, 1, 0);
        boostTimer = 0.2f;
    }
    void Update()
    {
        boostTimer -= Time.deltaTime;
        boostTimer = Mathf.Clamp(boostTimer, 0, 2f);
        if(boostTimer == 0 && !canBoost)
        {
            canBoost = true;
            CanBoostEffect();
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartGame();
            Restart();
            SetHandTarget();
        }
    }

    private void StartGame()
    {
        if (gamestarted) return;
        gamestarted = true;
        startScreen.SetActive(false);
        Time.timeScale = 1;
    }

    private void CanBoostEffect()
    {
        leftEyeMR.material.SetColor("_EmissionColor", boostEmissionCLR * 60);
        leftEyeMR.material.EnableKeyword("_EMISSION");

        rightEyeMR.material.SetColor("_EmissionColor", boostEmissionCLR * 60);
        rightEyeMR.material.EnableKeyword("_EMISSION");

        hairMR.material.SetColor("_EmissionColor", boostEmissionCLR * 60);
        hairMR.material.EnableKeyword("_EMISSION");
    }
    private void Boost()
    {
        leftEyeMR.material.SetColor("_EmissionColor", blueEmissionCLR);
        leftEyeMR.material.EnableKeyword("_EMISSION");

        rightEyeMR.material.SetColor("_EmissionColor", blueEmissionCLR);
        rightEyeMR.material.EnableKeyword("_EMISSION");

        hairMR.material.SetColor("_EmissionColor", blueEmissionCLR);
        hairMR.material.EnableKeyword("_EMISSION");

        canBoost = false;
        boostTimer = 2f;
        FindObjectOfType<AudioSource>().Play();
    }

    private void SetHandTarget()
    {
        GameObject obj = camCont.GetClickedObject();

        if (obj == null) return; 
        if(!leftHandC.onGrip && !rightHandC.onGrip)
        {
            if (canBoost) Boost();
            else return;
        }

        float objX = obj.transform.position.x,
            hipsX = playerHips.transform.position.x;
        if ((objX - hipsX) < (hipsX - objX))
        {
            focusOnRight = false;
            leftHandC.SetTarget(obj.transform);
        }
        else
        {
            focusOnRight = true;
            rightHandC.SetTarget(obj.transform);
        }
        leftHandC.Activate();
        rightHandC.Activate();

        if ((leftHandC.Target.position - rightHandC.Target.position).magnitude > 1.5f)
        {
            DropGrip(focusOnRight);
        }
    }
    private void DropGrip(bool dropLeft)
    {
        if (dropLeft) 
        {
            leftHandC.Disable();
            rightHandC.Activate();
        }
        else
        {
            leftHandC.Activate();
            rightHandC.Disable();
        }
    }
    public void DropGrip()
    {
        leftHandC.Disable();
        rightHandC.Disable();
    }

    public void Defeat()
    {
        if (gameover) return;
        gameover = true;
        endScreen.SetActive(true);
        Debug.Log("Fail");
        camCont.enabled = false;
        leftHandC.Disable();
        rightHandC.Disable();
    }
    public void Restart()
    {
        if (!gameover) return;
        SceneManager.LoadScene(0);
    }
}