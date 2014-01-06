using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiJack.Domain
{
    public class DealingShoe : NotifyPropertyChangeBase
    {
        private const int NUMBER_OF_DECKS = 2;

        public List<Card> Cards = new List<Card>();

        private int _numberOfCardsLeft;
        public int NumberOfCardsLeft
        {
            get { return _numberOfCardsLeft; }
            set
            {
                _numberOfCardsLeft = value;
                NotifyPropertyChange("NumberOfCardsLeft");
            }
        }

        public DealingShoe()
        {
            AddDecks();

            NumberOfCardsLeft = Cards.Count;
        }

        private void AddDecks()
        {
            for (int i = 1; i <= NUMBER_OF_DECKS; i++)
            {
                Cards.AddRange(new Deck().Cards);
            }
        }

        public void Shuffle()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Cards.Count; i++)
            {
                SwapCard(i, rand.Next(Cards.Count));
            }
        }

        public Card GetCard(bool hidden = false)
        {
            if (NumberOfCardsLeft == 0)
            {
                AddDecks();
                Shuffle();
            }
            var card = Cards[0];
            Cards.RemoveAt(0);
            card.Hidden = hidden;

            NumberOfCardsLeft = Cards.Count;

            return card;
        }

        private void SwapCard(int firstIndex, int secondIndex)
        {
            Card temp = Cards[firstIndex];
            Cards[firstIndex] = Cards[secondIndex];
            Cards[secondIndex] = temp;
        }
    }
}
