using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
namespace Assets
{
    public class Animations : MonoBehaviour
    {
        public GameObject helpPanel;
        public GameObject raisePanel;
        public GameObject buttonsPanel;
        public UnityEngine.UI.Slider MySlider;
        public TextMeshProUGUI helpText;
        public TextMeshProUGUI raiseInput;

        List<GameObject> cards = new();
        List<GameObject> players = new();
        List<GameObject> playersCardsPositions = new();
        List<CardPosition> communityCardsPositions = new();

        Game game = new();

        float startingY;
        float startingX;
        float startingZ;

        // Start is called before the first frame update
        void Start()
        {
            GameObject deck = GameObject.Find("Mazzo");
            GameObject Players = GameObject.Find("Players");
            GameObject CommunitySlots = GameObject.Find("CommunitySlots");
            for (int i = 0; i < deck.transform.childCount; i++)
            {
                GameObject card = deck.transform.GetChild(i).gameObject;
                cards.Add(card);
                game.Cards.Add(new(card));
            }
            startingY = cards[0].transform.position.y;
            startingX = cards[0].transform.position.x;
            startingZ = cards[0].transform.position.z;
            for (int i = 0; i < Players.transform.childCount; i++)
            {
                GameObject player = Players.transform.GetChild(i).gameObject;
                Bot p = new(i);
                for (int j = 0; j < player.transform.childCount; j++)
                {
                    playersCardsPositions.Add(player.transform.GetChild(j).gameObject);
                    p.positions.Add(new(player.transform.GetChild(j).position, Players.transform.GetChild(i).rotation));
                }
                players.Add(player);
                game.AddPlayer(p);
            }
            for (int i = 0; i < CommunitySlots.transform.childCount; i++)
            {
                GameObject communitySlot = CommunitySlots.transform.GetChild(i).gameObject;
                communityCardsPositions.Add(new(communitySlot.transform.position, communitySlot.transform.rotation));
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void StartGame()
        {
            StartCoroutine(ShuffleDeck((int)MySlider.value));
        }
        
        IEnumerator ShuffleDeck(int playersCount)
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
                iTween.MoveTo(card, new Vector3(startingX - 10, startingY + i, startingZ), .1f);
                i += .01f;
                yield return new WaitForSeconds(.1f);
            }
            i = 0;
            foreach (GameObject card in half2)
            {
                iTween.MoveTo(card, new Vector3(startingX + 10, startingY + i, startingZ), .1f);
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
            yield return StartCoroutine(DealCards(playersCount));
            buttonsPanel.SetActive(true);
            yield return StartCoroutine(Round(1, (int)MySlider.value));
        }

        IEnumerator DealCards(int players)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j <= players; j++)
                {
                    game.Players[j - 1].cards.Add(game.Cards[i * players + j - 1]);
                    iTween.MoveTo(cards[i * players + j - 1], new(game.Players[j - 1].positions[i].x, game.Players[j - 1].positions[i].y + i, game.Players[j - 1].positions[i].z), .5f);
                    iTween.RotateTo(cards[i * players + j - 1], iTween.Hash("x", game.Players[j - 1].positions[i].xRotation, "y", game.Players[j - 1].positions[i].yRotation, "z", game.Players[j - 1].positions[i].zRotation - 180, "time", .5f));
                    yield return new WaitForSeconds(.5f);
                }
            }
        }
        public void GetHandEvaluation()
        {
            if (!raisePanel.activeSelf && !helpPanel.activeSelf) // entrambi giù
            {
                helpPanel.SetActive(true);
            }
            else if (raisePanel.activeSelf)
            {
                raisePanel.SetActive(false);
                helpPanel.SetActive(true);
            }
            else if (helpPanel.activeSelf)
            {
                helpPanel.SetActive(false);
            }
            if (helpPanel.activeSelf)
                helpText.text = game.CalculateHandStrength(game.Players[0]);
        }
        public IEnumerator Round(int round, int players)
        {
            switch (round)
            {
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        iTween.MoveTo(cards[2 * players + i], communityCardsPositions[i].ToVector3(), .5f);
                        yield return new WaitForSeconds(.5f);
                        iTween.RotateTo(cards[2 * players + i], iTween.Hash("x", communityCardsPositions[i].xRotation, "y", communityCardsPositions[i].yRotation, "z", communityCardsPositions[i].zRotation - 180, "time", .2f));
                        yield return new WaitForSeconds(.2f);
                    }
                    break;
                case 2:
                    for (int i = 3; i < 4; i++)
                    {
                        iTween.MoveTo(cards[2 * players + i], communityCardsPositions[i].ToVector3(), .5f);
                        yield return new WaitForSeconds(.5f);
                        iTween.RotateTo(cards[2 * players + i], iTween.Hash("x", communityCardsPositions[i].xRotation, "y", communityCardsPositions[i].yRotation, "z", communityCardsPositions[i].zRotation - 180, "time", .2f));
                        yield return new WaitForSeconds(.2f);
                    }
                    break;
                case 3:
                    for (int i = 4; i < 5; i++)
                    {
                        iTween.MoveTo(cards[2 * players + i], communityCardsPositions[i].ToVector3(), .5f);
                        yield return new WaitForSeconds(.5f);
                        iTween.RotateTo(cards[2 * players + i], iTween.Hash("x", communityCardsPositions[i].xRotation, "y", communityCardsPositions[i].yRotation, "z", communityCardsPositions[i].zRotation - 180, "time", .2f));
                        yield return new WaitForSeconds(.2f);
                    }
                    break;
                default:
                    break;
            }
        }
        public void RaisePanel()
        {
            if(!raisePanel.activeSelf && !helpPanel.activeSelf) // entrambi giù
            {
                raisePanel.SetActive(true);
            }
            else if (helpPanel.activeSelf)
            {
                helpPanel.SetActive(false);
                raisePanel.SetActive(true);
            }
            else if (raisePanel.activeSelf)
            {
                raisePanel.SetActive(false);
            }
        }
        public void RaiseConfirm(string s)
        {
            if (int.TryParse(s, out int amount))
            {
                raiseInput.text = string.Empty;//non ho idea del perchè non funzioni, nella funzione getHandEvaluation ho usato lo stesso metodo e funziona 
                raisePanel.SetActive(false);
                game.PlayerRaise(amount);
            }
            else
            {
                Debug.LogError("Invalid input: " + s);
            }
            //raisePanel.SetActive(false);
        }

        public void Call()
        {
            game.PlayerCall();
        }
        public void Check()
        {
            game.PlayerCheck();
        }
        public void Fold()
        {
            game.PlayerFold();
        }
    }
}
