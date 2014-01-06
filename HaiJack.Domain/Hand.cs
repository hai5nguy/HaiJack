using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HaiJack.Domain
{
    public class Hand : NotifyPropertyChangeBase
    {
        #region Properties/Fields

        public Guid Id { get; set; }

        private ObservableCollection<Card> _cards = new ObservableCollection<Card>();
        public ObservableCollection<Card> Cards
        {
            get { return _cards; }
            set
            {
                _cards = value;
                NotifyPropertyChange("Cards");
            }
        }

        public HandStatus Status { get; set; }

        #endregion

        #region UI Messages

        private bool _showWonMessage = false;
        public bool ShowWonMessage
        {
            get { return _showWonMessage; }
            set
            {
                _showWonMessage = value;
                NotifyPropertyChange("ShowWonMessage");
            }
        }

        private bool _showLostMessage = false;
        public bool ShowLostMessage
        {
            get
            {
                return _showLostMessage;
            }
            set
            {
                _showLostMessage = value;
                NotifyPropertyChange("ShowLostMessage");
            }
        }

        private bool _showPushMessage = false;
        public bool ShowPushMessage
        {
            get { return _showPushMessage; }
            set
            {
                _showPushMessage = value;
                NotifyPropertyChange("ShowPushMessage");
            }
        }

        #endregion

        #region UI Data 

        private bool _active;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                NotifyPropertyChange("Active");
            }
        }

        private int _value = 0;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                NotifyPropertyChange("Value");
            }
        }

        private bool _showValue = true;
        public bool ShowValue
        {
            get
            {
                return _showValue;
            }
            set
            {
                _showValue = value;
                NotifyPropertyChange("ShowValue");
            }
        }

        private double _currentBet = 0;
        public double CurrentBet
        {
            get
            {
                return _currentBet;
            }
            set
            {
                _currentBet = value;
                NotifyPropertyChange("CurrentBet");
            }
        }

        #endregion

        #region Constructor

        public Hand(bool showValue = true)
        {
            Id = Guid.NewGuid();
            ShowValue = showValue;
            ClearCardsAndStatus();
        }

        #endregion

        #region Methods

        private void UpdateValue()
        {

            if (Cards == null) Value = 0;

            int total = 0;
            bool aceFound = false;
            foreach (var card in Cards)
            {
                if (card.Rank == 1) aceFound = true;

                total += card.GetValue();
            }

            if (total <= 11 && aceFound) total += 10;

            Value = total;
        }

        public void AddCard(Card card)
        {
            var newCard = card;
            newCard.ImageMargin = string.Format("{0},0,0,0", (Cards.Count == 0) ? "0" : "-50");
            Cards.Add(card);
            UpdateValue();
        }

        public void RemoveCard(int cardIndex)
        {
            Cards.RemoveAt(cardIndex);
            UpdateValue();
        }

        //public bool Splitable()
        //{
        //    return (Cards.Count == 2 && Cards[0].Rank == Cards[1].Rank) ? true : false;
        //}

        //public bool HasTwoAces()
        //{
        //    return (Splitable() && Cards[0].Rank == 1) ? true : false;
        //}

        public void ClearCardsAndStatus()
        {
            ResetMessages();
            Cards = new ObservableCollection<Card>();
            UpdateValue();
        }

        private void ResetMessages()
        {
            ShowWonMessage = false;
            ShowLostMessage = false;
            ShowPushMessage = false;

        }

        public void SetBet(double amount)
        {
            CurrentBet = amount;
        }

        public void SetStand()
        {
            Status = HandStatus.Standing;
        }
        public void SetWon()
        {
            CurrentBet = 0;
            Status = HandStatus.Finished;
            ShowWonMessage = true;
        }
        public void SetLost()
        {
            CurrentBet = 0;
            Status = HandStatus.Finished;
            ShowLostMessage = true;
        }
        public void SetPush()
        {
            Status = HandStatus.Finished;
            ShowPushMessage = true;
        }

        public bool IsStanding()
        {
            return (Status == HandStatus.Standing);
        }

        public bool IsBlackjack()
        {
            return (Status == HandStatus.Playable && Cards.Count == 2 && Value == 21);
        }

        public bool IsSplitable()
        {
            return (Status == HandStatus.Playable && Cards.Count == 2 && Cards[0].Rank == Cards[1].Rank);
        }

        public bool IsValueEleven()
        {
            return (Status == HandStatus.Playable && Cards.Count == 2 && Value == 11);
        }

        public bool IsTwentyOne()
        {
            return (Status == HandStatus.Playable && Value == 21);
        }

        public bool IsBust()
        {
            return (Status == HandStatus.Playable && Value > 21);
        }

        public void SetActive(bool active = true)
        {
            Active = active;
        }

        #endregion

        #region operator overloads

        public static bool operator <(Hand firstHand, Hand secondHand)
        {
            if ((object)firstHand == null || (object)secondHand == null) throw new NullReferenceException("Hand null in operator");
            var f = firstHand.Value;
            var s = secondHand.Value;
            return ((f <= 21 && s <= 21 && f < s) || (f > 21 && s <= 21)) ? true : false;
        }

        public static bool operator >(Hand firstHand, Hand secondHand)
        {
            if ((object)firstHand == null || (object)secondHand == null) throw new NullReferenceException("Hand null in operator");
            var f = firstHand.Value;
            var s = secondHand.Value;
            return ((f <= 21 && s <= 21 && f > s) || (f <= 21 && s > 21)) ? true : false;
        }

        public static bool operator ==(Hand firstHand, Hand secondHand)
        {
            if ((object)firstHand == null || (object)secondHand == null) throw new NullReferenceException("Hand null in operator");
            var f = firstHand.Value;
            var s = secondHand.Value;
            return ((f <= 21 && s <= 21 && f == s) || (f > 21 && s > 21)) ? true : false;
        }

        public static bool operator !=(Hand firstHand, Hand secondHand)
        {
            if ((object)firstHand == null || (object)secondHand == null) throw new NullReferenceException("Hand null in operator");
            var f = firstHand.Value;
            var s = secondHand.Value;
            return (f <= 21 && s <= 21 && f != s) ? true : false;
        }

        #endregion


    }

    public enum HandStatus { NoCards, Playable, Standing, Finished }
}
