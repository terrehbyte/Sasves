using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Order
{
    public int ind;
    public List<int> wavePlaces;

    public Order()
    {
        ind = 0;
        wavePlaces = new List<int>();
    }

    public static int GetMaxOperations(List<Order> orders)
    {
        int retval = orders.Count;

        foreach(var order in orders)
        {
            retval += order.wavePlaces.Count;
        }

        return retval;
    }
}

public class Simon : MonoBehaviour {

    [System.Serializable]
    public struct Config
    {
        public int numObjects;
        public int maxOrderWaves;
        public int minOrderWaves;
        public int maxOrders;
        public int minOrders;
        public bool noDupes;
        public bool easyMode;
    }

    public Config gameConfig;

    public Image orderImage;
    public Timer transitionTimer;
    public Timer alphTransitionTimer;
    public Timer preparationTimer;
    public Image vignetteImage;
    public Timer vignetteFlashTimer;
    public List<Image> CirclePiColors;

    public List<Order> orders = new List<Order>();

    private bool displayingOrder = false;
    private bool transitioning = false;
    private bool transitionAlph = false;
    private int curOrder = 0;
    private int curWave = 0;

    public UnityEvent onTransitionStart;
    public UnityEvent onTransitionExit;

    private Quaternion[] angles = new Quaternion[] { Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f)), Quaternion.Euler(Vector3.zero), Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f)) };

    public bool isCurrentlyDisplaying { get { return displayingOrder; } }



	// Use this for initialization
	void Start () {
        transitionTimer.init();
        alphTransitionTimer.init();
        vignetteFlashTimer.init();
        preparationTimer.init();
	}
	
	// Update is called once per frame
	void Update () {
        transitionTimer.update();
        alphTransitionTimer.update();
        vignetteFlashTimer.update();
        preparationTimer.update();

        if(!preparationTimer.isPassed())
        {
            transitionTimer.resetTimer();
        }

        if (displayingOrder && preparationTimer.isPassed())
        {
            if (transitioning)
            {
                if (curOrder < orders.Count && curWave < orders[curOrder].wavePlaces.Count)
                    doTransition();
                if (transitionTimer.isPassed())
                {
                    transitionTimer.resetTimer();

                    transitionTimer.setActive(true);
                    curWave++;
                    if (curOrder < orders.Count - 1)
                    {
                        if (curWave >= orders[curOrder].wavePlaces.Count)
                        {
                            curOrder++;
                            vignetteImage.color = ColorDB.manager.colorDB[orders[curOrder].ind];
                            CirclePiColors[orders[curOrder].ind].enabled = true;
                            vignetteFlashTimer.resetTimer();
                            vignetteFlashTimer.setActive(true);

                            orderImage.sprite = SpriteDB.manager.spriteDB[orders[curOrder].ind];
                            curWave = 0;
                        }
                        else
                            transitioning = false;
                    }
                    else if (curWave >= orders[curOrder].wavePlaces.Count)
                        exitTrans();
                }
            }
            else
            {
                if (transitionTimer.isPassed())
                {
                    transitionTimer.resetTimer();
                    transitioning = true;
                    transitionTimer.setActive(true);
                }
            }
            if (transitionAlph)
            {
                orderImage.color = Color.Lerp(orderImage.color, Color.white, alphTransitionTimer.percentPassed());
            }
            else
            {
                orderImage.color = Color.Lerp(orderImage.color, new Color(1.0f, 1.0f, 1.0f, 0.0f), alphTransitionTimer.percentPassed());
            }
        }

        if(vignetteFlashTimer.isPassed())
        {
            vignetteImage.color = ColorDB.manager.colorDB[4];
            foreach (var colorGlow in CirclePiColors)
            {
                colorGlow.enabled = false;
            }
        }

	}

    public void exitTrans()
    {
        displayingOrder = false;
        transitionAlph = false;
        alphTransitionTimer.resetTimer();
        alphTransitionTimer.setActive(true);
        orderImage.color = ColorDB.manager.colorDB[4];
        onTransitionExit.Invoke();
    }

    public void prepareRound()
    {
        GenerateRound();
        prepareTransition();
    }

    private void prepareTransition()
    {
        onTransitionStart.Invoke();
        displayingOrder = true;
        curOrder = 0;
        curWave = 0;
        transitioning = true;
        transitionAlph = true;
        transitionTimer.resetTimer();
        transitionTimer.setActive(true);
        orderImage.sprite = SpriteDB.manager.spriteDB[orders[curOrder].ind];
        vignetteImage.color = ColorDB.manager.colorDB[orders[curOrder].ind];
        CirclePiColors[orders[curOrder].ind].enabled = true;
        vignetteFlashTimer.resetTimer();
        vignetteFlashTimer.setActive(true);
        preparationTimer.resetTimer();
        preparationTimer.setActive(true);
    }

    private void finishTransition()
    {
        transitionAlph = false;
        orderImage.transform.rotation = angles[1];
    }

    private void doTransition()
    {
        orderImage.transform.rotation = Quaternion.Slerp(orderImage.transform.rotation, angles[orders[curOrder].wavePlaces[curWave]], transitionTimer.percentPassed());
    }

    public void GenerateRound()
    {
        orders = new List<Order>();
        int numOrders = Random.Range(gameConfig.minOrders, gameConfig.maxOrders);
        for (int i = 0; i < numOrders; i++)
        {
            int numWaves = Random.Range(gameConfig.minOrderWaves, gameConfig.maxOrderWaves);
            GeneratePattern(numWaves);
        }
    }

    public void GeneratePattern(int maxWaves)
    {
        Order simonOrder = new Order();
        List<int> available = new List<int> { 0, 1, 2, 3 };
        if (gameConfig.noDupes)
        {
            for (int i = 0; i < orders.Count; i++)
                available.Remove(orders[i].ind);
        }
        if (available.Count == 0)
            return;
        simonOrder.ind = available[Random.Range(0, available.Count)];
        if(!gameConfig.noDupes && gameConfig.easyMode)
        {
            Order tmp = orders.Find(x => (x.ind == simonOrder.ind));
            if (tmp != null)
            {
                simonOrder.wavePlaces = tmp.wavePlaces;
                orders.Add(simonOrder);
                return;
            }
        }
        simonOrder.wavePlaces.Add(Random.Range(0, 3));
        for (int i = 0; i < maxWaves - 1; i++)
        {
            List<int> choices = new List<int> { 0, 1, 2 };
            choices.Remove(simonOrder.wavePlaces[simonOrder.wavePlaces.Count - 1]);
            simonOrder.wavePlaces.Add(choices[Random.Range(0, choices.Count)]);
        }
        orders.Add(simonOrder);
    }

    public int MaxRight()
    {
        return MaxOps(orders);
    }

    public int MaxOps(List<Order> orderIn)
    {
        return Order.GetMaxOperations(orderIn);
        //int ret = orderIn.Count;
        //for (int i = 0; i < orderIn.Count; i++)
        //{
        //    ret += orderIn[i].wavePlaces.Count;
        //}
        //return ret;
    }

    public int CheckRight(List<Order> orderCheck)
    {
        int ret = 0;

        for (int i = 0; i < orders.Count && i < orderCheck.Count; i++)
        {

            // check type
            if (orders[i].ind == orderCheck[i].ind)
                ret++;
            else
                continue;   // they can't get the rest right

            // check to see if they gave the correct movements
            for (int j = 0; j < orders[i].wavePlaces.Count && j < orderCheck[i].wavePlaces.Count; j++)
            {
                if (orders[i].wavePlaces[j] == orderCheck[i].wavePlaces[j])
                    ret++;
            }

        }

        return ret;
    }

    public int CheckWrong(List<Order> orderCheck)
    {
        int checkRight = CheckRight(orderCheck);
        int againstSelf = MaxOps(orderCheck) - checkRight,
            againstSimon = MaxRight() - checkRight;
        return  againstSelf > againstSimon ? againstSelf : againstSimon;
    }
}
