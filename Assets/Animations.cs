using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Player
{
    public int id { get; set; }
    public List<Cards> cards { get; set; }
    public List<CardPosition> positions { get; set; }
    public Player(int id) { this.id = id; cards = new List<Cards>(); positions = new List<CardPosition>(); }
}
public class Cards
{
    public string suit { get; set; }
    public int value { get; set; }
    public string name { get; set; }
    public Cards(string suit, int value, string name)
    {
        this.suit = suit;
        this.value = value;
        this.name = name;
    }
}
public class CardPosition
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float xRotation { get; set; }
    public float yRotation { get; set; }
    public float zRotation { get; set; }
    public CardPosition(float x, float y, float z, float xRotation, float yRotation, float zRotation)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.xRotation = xRotation;
        this.yRotation = yRotation;
        this.zRotation = zRotation;
    }
    public CardPosition(Vector3 position, Vector3 rotation)
    {
        x = position.x;
        y = position.y;
        z = position.z;
        xRotation = rotation.x;
        yRotation = rotation.y;
        zRotation = rotation.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector3 ToVectorRotation()
    {
        return new Vector3(xRotation, yRotation, zRotation);
    }
}
public class Animations : MonoBehaviour
{
    List<GameObject> cards = new();
    List<GameObject> players = new();
    List<GameObject> playersCardsPositions = new();
    List<Player> playerList = new();

    float startingY;
    float startingX;
    float startingZ;

    // Start is called before the first frame update
    void Start()
    {
        GameObject deck = GameObject.Find("Mazzo");
        GameObject Players = GameObject.Find("Players");
        for (int i = 0; i < deck.transform.childCount; i++)
        {
            GameObject card = deck.transform.GetChild(i).gameObject;
            cards.Add(card);
        }
        startingY = cards[0].transform.position.y;
        startingX = cards[0].transform.position.x;
        startingZ = cards[0].transform.position.z;
        for (int i = 0; i < Players.transform.childCount; i++)
        {
            GameObject player = Players.transform.GetChild(i).gameObject;
            Player p = new(i);
            for (int j = 0; j < player.transform.childCount; j++)
            {
                playersCardsPositions.Add(player.transform.GetChild(j).gameObject);
                p.positions.Add(new(player.transform.GetChild(j).position, Players.transform.GetChild(i).rotation.ToEulerAngles()));
                p.positions.Add(new(player.transform.GetChild(j).position, new(Players.transform.GetChild(i).rotation.x, Players.transform.GetChild(i).rotation.y, Players.transform.GetChild(i).rotation.z)));
            }
            players.Add(player);
            playerList.Add(p);
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
        float i = 0;
        foreach (GameObject card in half1)
        {
            iTween.MoveTo(card, new Vector3(startingX-10, startingY + i, startingZ), .1f);
            i += .01f;
            yield return new WaitForSeconds(.1f);
        }
        i = 0;
        foreach (GameObject card in half2)
        {
            iTween.MoveTo(card, new Vector3(startingX+10, startingY + i, startingZ), .1f);
            i += .01f;
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
        i = 0;
        foreach (GameObject card in cards)
        {
            iTween.MoveTo(card, new Vector3(startingX, startingY + i, startingZ), .2f);
            i += .04f;
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(.5f);
        StartCoroutine(DealCards(players.Count));
    }

    IEnumerator DealCards(int players)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 1; j <= players; j++)
            {
                iTween.MoveTo(cards[i * players + j - 1], playerList[j - 1].positions[i].ToVector3(), .5f);
                iTween.RotateTo(cards[i * players + j - 1], new(playerList[j - 1].positions[i].xRotation, playerList[j - 1].positions[i].yRotation, playerList[j - 1].positions[i].zRotation - 180), .5f);
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
