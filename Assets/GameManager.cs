using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int TotalScore;
    public int Health = 100;
    public static GameManager ManagerInstance;
    public GameObject Player;
    public void Awake()
    {
        ManagerInstance = this;

    }
    public void DamagePlayer(int Count)
    {
        if (Health > 0)
        {
            Health -= Count;
            Debug.Log("��� �������� ����� � ������� " + Count);
        }
        else if (Health <= 0) Debug.Log("������");
    }
    public void AddScore(int Count)
    {
        TotalScore += Count;
        Debug.Log("�������� ����: " + TotalScore);
    }
}
