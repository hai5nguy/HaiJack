using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiJack.Domain
{
    public class Deck
    {
        private const string IMAGE_PATH = @"..\Images\";

        public List<Card> Cards = new List<Card>();

        public Deck()
        {
            var suits = new List<Suit>() { Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade };

            int imageIndex = 1;
            foreach (var suit in suits)
            {
                for (int i = 1; i <= 13; i++)
                {
                    Cards.Add(new Card()
                    {
                        Suit = suit, 
                        Rank = i, 
                        ImageUri = IMAGE_PATH + imageIndex.ToString() + @".png"
                    });
                    imageIndex++;
                }
            }
        }
    }
}
