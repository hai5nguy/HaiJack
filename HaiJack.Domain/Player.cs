using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using HaiJack.Domain.ExtensionMethods;

namespace HaiJack.Domain
{
    public class Player : NotifyPropertyChangeBase
    {
        #region Properties 

        public Guid Id { get; set; }
        public string Name { get; set; }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                NotifyPropertyChange("Active");
            }
        }

        public ObservableCollection<Hand> Hands { get; set; }

        public Hand ActiveHand
        {
            get { return Hands.Single(h => h.Active); }
        }

        public double _bank = 1000;
        public double Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
                NotifyPropertyChange("Bank");
            }
        }

        #region UI Controls

        private bool _showBetControls;
        public bool ShowBetControls
        {
            get { return _showBetControls; }
            set
            {
                _showBetControls = value;
                NotifyPropertyChange("ShowBetControls");
            }
        }

        private bool _showActionControls;
        public bool ShowActionControls
        {
            get { return _showActionControls; }
            set
            {
                _showActionControls = value;
                NotifyPropertyChange("ShowActionControls");
            }
        }

        private bool _doubleDownAvailable;
        public bool DoubleDownAvailable
        {
            get
            {
                return _doubleDownAvailable;
            }
            set
            {
                _doubleDownAvailable = value;
                NotifyPropertyChange("DoubleDownAvailable");
            }
        }

        private bool _splitAvailable;
        public bool SplitAvailable
        {
            get
            {
                return _splitAvailable;
            }
            set
            {
                _splitAvailable = value;
                NotifyPropertyChange("SplitAvailable");
            }
        }

        #endregion

        #endregion

        public Player(string name, bool active = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Hands = new ObservableCollection<Hand>();
            Hands.Add(new Hand() { Status = HandStatus.NoCards, Active = true });
            ShowBetControls = true;
        }

        #region methods

        public void ReceiveCard(Card card)
        {
            Hands.Single(h => h.Active).AddCard(card);
        }

        public void SetBetOnHand(Guid handId, double amount)
        {
            if (amount > Bank) return;
            Bank -= amount;
            Hands.Single(h => h.Id == handId).SetBet(amount);

            ShowBetControls = false;
        }

        public void StandHand(Guid handId)
        {
            Hands.Single(h => h.Id == handId).SetStand();
        }

        public void HitOnActiveHand()
        {
            var activeHand = Hands.Single(h => h.Active);
            activeHand.AddCard(Game.TheShoe.GetCard());
        }

        public void StandOnActiveHand()
        {
            ActiveHand.Status = HandStatus.Standing;
        }

        public void WinHand(Guid handId)
        {
            var hand = Hands.Single(h => h.Id == handId);
            Bank += hand.CurrentBet * 2;
            hand.SetWon();

            ShowBetControls = true;
            
        }
        public void LoseHand(Guid handId)
        {
            var hand = Hands.Single(h => h.Id == handId);
            hand.SetLost();

            ShowBetControls = true;
        }
        public void PushHand(Guid handId)
        {
            var hand = Hands.Single(h => h.Id == handId);
            Bank += hand.CurrentBet;
            hand.CurrentBet = 0;
            hand.SetPush();

            ShowBetControls = true;
        }

        public void PlayerWonWithBlackJack(Guid handId)
        {
            var hand = Hands.Single(h => h.Id == handId);
            Bank += hand.CurrentBet * 2.5;
            hand.CurrentBet = 0;
            hand.SetWon();
        }

        public void DoublingBet(Guid handId)
        {
            var hand = Hands.Single(h => h.Id == handId);
            Bank -= hand.CurrentBet;
            hand.CurrentBet += hand.CurrentBet;
        }

        public void SplitHand()
        {
            var activeHand = Hands.Single(h => h.Active);
            var newCard = new Card()
            {
                Hidden = false,
                ImageMargin = "0,0,0,0",
                ImageUri = activeHand.Cards[1].ImageUri,
                Rank = activeHand.Cards[1].Rank,
                Suit = activeHand.Cards[1].Suit,
            };
            var newHand = new Hand()
            {
                Status = HandStatus.Playable
            };
            newHand.AddCard(newCard);

            var activeIndex = Hands.ToList().FindIndex(h => h.Active);
            Hands.Insert(activeIndex + 1, newHand);
            Hands[activeIndex].RemoveCard(1);


            Hands[activeIndex].AddCard(Game.TheShoe.GetCard());
            Hands[activeIndex + 1].AddCard(Game.TheShoe.GetCard());

            SetBetOnHand(Hands[activeIndex + 1].Id, activeHand.CurrentBet);

            SplitAvailable = false;
        }

        #endregion


        public void ClearHands()
        {
            while (Hands.Count > 1)
            {
                Hands.RemoveAt(1);
            }
            Hands[0].ClearCardsAndStatus();
            Hands[0].SetActive();
            SplitAvailable = false;
            DoubleDownAvailable = false;
        }

        public void SetActive(bool active = true)
        {
            Active = active;
            ShowActionControls = active;
        }
    }
}
