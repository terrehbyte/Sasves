using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersInput : MonoBehaviour
{
    public event EventHandler<EventArgs> OnWavedToLeft;
    public event EventHandler<EventArgs> OnWavedToCenter;
    public event EventHandler<EventArgs> OnWavedToRight;
    public event EventHandler<EventArgs> OnPlayerClickedDone;

    public Image guide;

    private List<Order> playerWaves = new List<Order>();
    public Timer waveEndTimer;

    public bool isDone { get; private set; }

    public bool IsFollowingSimon;

    public AudioClip LeftAudio;
    public AudioClip CenterAudio;
    public AudioClip RightAudio;

    private AudioSource audioSrc;
    private int lastWave;

    private float rotationOffset;
    

    private Quaternion[] angles = new Quaternion[] { Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f)), Quaternion.Euler(Vector3.zero), Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f)) };

    void Start()
    {
       // Calibrate();
        audioSrc = this.GetComponent<AudioSource>();
        waveEndTimer.init();
    }

    //public void Calibrate()
    //{
    //    rotationOffset = 360 + 90 - Input.gyro.attitude.eulerAngles.z;
    //}

    void Update()
    {
        waveEndTimer.update();
        if (IsFollowingSimon)
        {
            recordPlayerWaves();
        }
    }

    void OnDisable()
    {
    }

    public List<Order> getPlayerInput()
    {
        return playerWaves;
    }

    private void resetWaveEnd()
    {
        if(!waveEndTimer.isPassed())
        {
            waveEndTimer.resetTimer();
        }
    }

    public void StartNewWave(int index)
    {
        Order newOrd = new Order();
        newOrd.ind = index;
        newOrd.wavePlaces = new List<int>();
        guide.color = ColorDB.manager.colorDB[index];
        guide.sprite = SpriteDB.manager.spriteDB[4];
        guide.transform.rotation = angles[1];
        playerWaves.Add(newOrd);
        resetWaveEnd();
        waveEndTimer.setActive(true);
    }

    public void PlayerClickedDone()
    {
        isDone = false;
        if (OnPlayerClickedDone != null)
            OnPlayerClickedDone(this, new EventArgs());
        IsFollowingSimon = false;
    }

    public void onEndRound()
    {
        playerWaves = new List<Order>();
        IsFollowingSimon = false;
        guide.color = ColorDB.manager.colorDB[5];
        isDone = false;
    }

    private void wavedToLeft()
    {
        playAudioClip(LeftAudio);
        Handheld.Vibrate();
        lastWave = 0;
        playerWaves[playerWaves.Count - 1].wavePlaces.Add(0);
        guide.transform.rotation = angles[0];
        if (OnWavedToLeft != null)
            OnWavedToLeft(this, new EventArgs());
        resetWaveEnd();
    }

    private void wavedToCenter()
    {
        playAudioClip(CenterAudio);
        Handheld.Vibrate();
        lastWave = 1;
        playerWaves[playerWaves.Count - 1].wavePlaces.Add(1);
        guide.transform.rotation = angles[1];
        if (OnWavedToCenter != null)
            OnWavedToCenter(this, new EventArgs());
        resetWaveEnd();
    }

    private void wavedToRight()
    {
        playAudioClip(RightAudio);
        Handheld.Vibrate();
        lastWave = 2;
        playerWaves[playerWaves.Count - 1].wavePlaces.Add(2);
        guide.transform.rotation = angles[2];
        if (OnWavedToRight != null)
            OnWavedToRight(this, new EventArgs());
        resetWaveEnd();
    }

    private void playAudioClip(AudioClip clip)
    {
        if (audioSrc != null)
        {
            audioSrc.Stop();
            audioSrc.clip = clip;
            audioSrc.Play();
        }
    }

    private void recordPlayerWaves()
    {
        // use accelo and gyro data to determine wave type
        if (waveEndTimer.isPassed() && IsFollowingSimon)
        {
            isDone = true;
        }
        else if (Mathf.Abs(Input.gyro.rotationRate.x) < .02 &&
            !waveEndTimer.isPassed())
        {
            var acceloRotation = Input.acceleration.x;
            //var rotation = Input.gyro.attitude.eulerAngles.z + rotationOffset;

            Debug.Log("lastWave" + lastWave + System.Environment.NewLine + "rotation" + acceloRotation);
            //if (rotation < (360 + 10) || rotation > (360 + 170))
            //{
            //    // do nothing, not a valid wave position
            //}
            //else if (rotation < (360 + 70) && lastWave != 2)
            //{
            //    wavedToRight();
            //}
            //else if (rotation > (360 + 110) && lastWave != 0)
            //{
            //    wavedToLeft();
            //}
            //else if (rotation > (360 + 70) && rotation < (360 + 110) && lastWave != 1)
            //{
            //    wavedToCenter();
            //}

            if (playerWaves.Count < 1)
                return;



            if (acceloRotation > .6 && lastWave != 2)
            {
                wavedToRight();
            }
            else if (acceloRotation < -.6 && lastWave != 0)
            {
                wavedToLeft();
            }
            else if (acceloRotation < .4 && acceloRotation > -.4 && lastWave != 1)
            {
                wavedToCenter();
            }
        }
    }
}
