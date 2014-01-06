using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using HaiJack.Domain.ExtensionMethods;

namespace HaiJack.Domain
{
    public class Dealer : NotifyPropertyChangeBase
    {
        public Hand Hand { get; set; }

        public Dealer()
        {
            Hand = new Hand(showValue: false);
        }

        public void ReceiveCard(Card card)
        {
            Hand.AddCard(card);
        }

        public void Play()
        {
            RevealHiddenCard();

            while (Hand.Value < 17)
            {
                ReceiveCard(Game.TheShoe.GetCard());
            }
        }

        public void RevealHiddenCard()
        {
            Hand.Cards[0].Hidden = false;
            Hand.Cards[1].ImageMargin = "0,0,0,0";
            Hand.ShowValue = true;
        }

        public void ClearHand()
        {
            Hand.ClearCardsAndStatus();
            Hand.ShowValue = false;
        }

    }
}
