using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HaiJack.UnitTest
{
    [TestClass]
    public class HandTests
    {
        [TestMethod]
        public void an_empty_hand_should_return_value_0()
        {
            var hand = new Hand();

            var value = hand.Value;

            Assert.AreEqual(0, value);

        }

        [TestMethod]
        public void an_10_and_jack_should_return_20()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 10 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 11 });

            Assert.AreEqual(20, hand.Value);
        }

        [TestMethod]
        public void an_ace_and_jack_should_return_21()
        {
            var hand = new Hand();

            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 11 });

            Assert.AreEqual(21, hand.Value);
        }

        [TestMethod]
        public void an_ace_and_king_should_return_21()
        {
            var hand = new Hand();

            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 13 });

            Assert.AreEqual(21, hand.Value);
        }
        [TestMethod]
        public void an_ace_and_5_should_return_value_16()
        {
            var hand = new Hand();

            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 5 });

            Assert.AreEqual(16, hand.Value);
        }

        [TestMethod]
        public void a_hand_with_2_aces_should_return_value_12()
        {
            var hand = new Hand();

            hand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });

            Assert.AreEqual(12, hand.Value);
        }
        [TestMethod]
        public void a_hand_with_8_aces_should_return_value_18()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Heart, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Heart, Rank = 1 });

            Assert.AreEqual(18, hand.Value);
        }

        [TestMethod]
        public void a_hand_with_blackjack_should_return_value_10_when_ace_is_removed()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            hand.AddCard(new Card() { Suit = Suit.Club, Rank = 10 });

            hand.RemoveCard(0);

            Assert.AreEqual(10, hand.Value);
        }

        [TestMethod]
        public void a_hand_value_21_should_win_vs_value_20()
        {
            var winningHand = new Hand();
            winningHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            winningHand.AddCard(new Card() { Suit = Suit.Club, Rank = 11 });

            var losingHand = new Hand();
            losingHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 11 });
            losingHand.AddCard(new Card() { Suit = Suit.Club, Rank = 11 });

            Assert.IsTrue(winningHand > losingHand);
        }

        [TestMethod]
        public void a_hand_value_15_should_lose_vs_value_19()
        {
            var winningHand = new Hand();
            winningHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 10 });
            winningHand.AddCard(new Card() { Suit = Suit.Club, Rank = 9 });

            var losingHand = new Hand();

            losingHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            losingHand.AddCard(new Card() { Suit = Suit.Club, Rank = 4 });

            Assert.IsTrue(winningHand > losingHand);
        }

        [TestMethod]
        public void a_hand_value_20_should_win_vs_a_bust()
        {
            var winningHand = new Hand();
            winningHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 10 });
            winningHand.AddCard(new Card() { Suit = Suit.Club, Rank = 11 });

            var losingHand = new Hand();
            losingHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            losingHand.AddCard(new Card() { Suit = Suit.Club, Rank = 4 });
            losingHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 10 });
            losingHand.AddCard(new Card() { Suit = Suit.Diamond, Rank = 10 });

            Assert.IsTrue(winningHand > losingHand);
        }

        [TestMethod]
        public void two_hands_with_equal_value_should_equal()
        {
            var firstHand = new Hand();
            firstHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 7 });
            firstHand.AddCard(new Card() { Suit = Suit.Club, Rank = 3 });
            firstHand.AddCard(new Card() { Suit = Suit.Club, Rank = 9 });
            firstHand.AddCard(new Card() { Suit = Suit.Club, Rank = 2 });

            var secondHand = new Hand();
            secondHand.AddCard(new Card() { Suit = Suit.Spade, Rank = 1 });
            secondHand.AddCard(new Card() { Suit = Suit.Club, Rank = 10 });

            Assert.IsTrue(firstHand == secondHand);
        }

        [TestMethod]
        public void a_hand_should_be_able_to_clear_itself()
        {
            var hand = new Hand();
            hand.AddCard(new Card() {Suit = Suit.Club, Rank = 1});
            hand.ShowLostMessage = true;
            hand.ShowPushMessage = true;
            hand.ShowWonMessage = true;

            hand.ClearCardsAndStatus();

            Assert.IsFalse(hand.ShowLostMessage);
            Assert.IsFalse(hand.ShowPushMessage);
            Assert.IsFalse(hand.ShowWonMessage);
            Assert.AreEqual(0, hand.Cards.Count);
            Assert.AreEqual(0, hand.Value);

        }

        [TestMethod]
        public void hand_should_be_able_to_set_win()
        {
            var hand = new Hand();
            hand.CurrentBet = 999;

            hand.SetWon();

            Assert.AreEqual(0, hand.CurrentBet);
            Assert.AreEqual(HandStatus.Finished, hand.Status);
            Assert.IsTrue(hand.ShowWonMessage);
        }

        [TestMethod]
        public void hand_should_be_able_to_set_lost()
        {
            var hand = new Hand();
            hand.CurrentBet = 999;

            hand.SetLost();

            Assert.AreEqual(0, hand.CurrentBet);
            Assert.AreEqual(HandStatus.Finished, hand.Status);
            Assert.IsTrue(hand.ShowLostMessage);
        }

        [TestMethod]
        public void hand_should_be_able_to_set_push()
        {
            var hand = new Hand();

            hand.SetPush();

            Assert.AreEqual(HandStatus.Finished, hand.Status);
            Assert.IsTrue(hand.ShowPushMessage);
        }

        [TestMethod]
        public void a_hand_should_be_able_to_determine_if_it_is_a_blackjack()
        {
            var hand = new Hand();
            hand.AddCard(new Card(){Rank = 1, Suit = Suit.Club});
            hand.AddCard(new Card(){Rank = 11, Suit = Suit.Club});
            hand.Status = HandStatus.Playable;
            
            var isBlackJack = hand.IsBlackjack();

            Assert.IsTrue(isBlackJack);
        }
        [TestMethod]
        public void a_hand_should_be_able_to_determine_if_it_is_splitable()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Rank = 3, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 3, Suit = Suit.Heart });
            hand.Status = HandStatus.Playable;

            var splitable = hand.IsSplitable();

            Assert.IsTrue(splitable);
        }

        [TestMethod]
        public void a_hand_should_be_able_to_determine_if_it_can_be_doubled_down()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Rank = 3, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 8, Suit = Suit.Heart });
            hand.Status = HandStatus.Playable;

            var ableToDoubleDown = hand.IsValueEleven();

            Assert.IsTrue(ableToDoubleDown);
        }
    }
}
