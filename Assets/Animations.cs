using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animations : MonoBehaviour
{
    List<GameObject> cards = new();

    // Start is called before the first frame update
    void Start()
    {
        GameObject deck = GameObject.Find("Mazzo");
        for (int i = 0; i < deck.transform.childCount; i++)
        {
            GameObject card = deck.transform.GetChild(i).gameObject;
            cards.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // pulsante sinistro del mouse cliccato
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShuffleDeck());
        }
    }
    IEnumerator ShuffleDeck()
    {
        // divide il mazzo in due parti uguali
        List<GameObject> half1 = cards.Take(cards.Count / 2).ToList();
        List<GameObject> half2 = cards.Skip(cards.Count / 2).ToList();

        // mischia le carte
        half1 = half1.OrderBy(x => Random.value).ToList();
        half2 = half2.OrderBy(x => Random.value).ToList();

        //animazione divisione in due mazzi
        float i = 1;
        foreach (GameObject card in half1)
        {
            iTween.MoveTo(card, new Vector3(-10, i, 0), .1f);
            i += .04f;
            yield return new WaitForSeconds(.1f);
        }
        i = 1;
        foreach (GameObject card in half2)
        {
            iTween.MoveTo(card, new Vector3(10, i, 0), .1f);
            i += .04f;
            yield return new WaitForSeconds(.1f);
        }

        //riunisco le carte
        for (int j = 0; j < cards.Count; j++)
        {
            if (j % 2 == 0)
                cards[j] = half1[j / 2];
            else
                cards[j] = half2[(j - 1) / 2];
        }

        yield return new WaitForSeconds(.5f);

        //carte tornano in un unico mazzo
        i = 1;
        foreach (GameObject card in cards)
        {
            iTween.MoveTo(card, new Vector3(0, i, 0), .2f);
            i += .04f;
            yield return new WaitForSeconds(.2f);
        }
    }
}
