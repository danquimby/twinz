using System.Linq;
using UnityEngine;

public class CardPackManager : MonoBehaviour
{
    [SerializeField] private int SetCount = 5;
    [SerializeField] private int SetCardCount = 7;
    [SerializeField] private int[,] CardSet;
    private int[] totalIds = new int[40];
    private int nSet = 0;
    
    void Start()
    {
        CardSet = new int[SetCount, SetCardCount];
        for (int i = 0; i < 40; i++)
            totalIds[i] = i + 1;
    }
    public void InitNewSet()
    {
        nSet = 0;
        int[] ids = totalIds.ToArray();
        
        // fill card set
        int count = 0;
        for (int i = 0; i < SetCount; i++)
        {
            for (int j = 0; j < SetCardCount; j++)
            {
                CardSet[i, j] = ids[count++];
            }
        }
    }

    public int[] GetSet()
    {
        int[] result = new int[SetCardCount*2];
        for (int i = 0, j=0; i < SetCardCount; i++, j+=2)
        {
            result[j] = CardSet[nSet, i];
            result[j+1] = CardSet[nSet, i];
        }
        // double mixing, maybe will be better
        mixedArray(ref result);
        mixedArray(ref result);

        if (++nSet > SetCount)
        {
            InitNewSet();   
        }
        return result;
    }

    private void mixedArray(ref int[] ids)
    {
        for (int i = 0; i < ids.Length; i++) {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Length);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }
    }

}
