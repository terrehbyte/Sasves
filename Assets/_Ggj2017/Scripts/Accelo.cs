using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accelo : MonoBehaviour
{
    public Text WaveType;
    public Text DebugText;

    private PlayersInput playerInput;
    private int waveCount;

    // Use this for initialization
    void Start()
    {
        playerInput = this.GetComponent<PlayersInput>();
        playerInput.IsFollowingSimon = true;
        playerInput.OnWavedToCenter += PlayerInput_OnWavedToCenter;
        playerInput.OnWavedToLeft += PlayerInput_OnWavedToLeft;
        playerInput.OnWavedToRight += PlayerInput_OnWavedToRight;
        playerInput.StartNewWave(0);
        Input.gyro.enabled = true;
    }

    private void PlayerInput_OnWavedToRight(object sender, System.EventArgs e)
    {
        WaveType.text = ++waveCount + System.Environment.NewLine +
            "Right";
    }

    private void PlayerInput_OnWavedToLeft(object sender, System.EventArgs e)
    {
        WaveType.text = ++waveCount + System.Environment.NewLine +
            "Left";
    }

    private void PlayerInput_OnWavedToCenter(object sender, System.EventArgs e)
    {
        WaveType.text = ++waveCount + System.Environment.NewLine +
            "Center";
    }

    // Update is called once per frame
    void Update()
    {
        var n = System.Environment.NewLine;
        DebugText.text = "Accelo:" + toPrecision(Input.acceleration)
            + n + "isGyro on?" + Input.gyro.enabled
            + n + "attitude:" + toPrecision(Input.gyro.attitude.eulerAngles)
            + n + "gravity:" + toPrecision(Input.gyro.gravity)
            + n + "rotationRate:" + toPrecision(Input.gyro.rotationRate)
            + n + "Unbiased:" + toPrecision(Input.gyro.rotationRateUnbiased);
        //transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
    }

    private void OnDisable()
    {
        playerInput.OnWavedToCenter -= PlayerInput_OnWavedToCenter;
        playerInput.OnWavedToLeft -= PlayerInput_OnWavedToLeft;
        playerInput.OnWavedToRight -= PlayerInput_OnWavedToRight;
    }

    private string toPrecision(Vector3 vector)
    {
        return string.Format("({0:0.000},{1:000},{2:000})", vector.x, vector.y, vector.z);
    }
}
