using System;
using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaiJack.UnitTest
{
    [TestClass]
    public class PlayerTests
    {
        private Card AceClub
        {
            get
            {
                return new Card() { Rank = 1, Suit = Suit.Club };
            }
        }

        [TestMethod]
        public void player_should_able_to_receive_card_to_active_hand()
        {
            var player = new Player("test player");

            player.ReceiveCard(AceClub);

            Assert.AreEqual(1, player.ActiveHand.Cards[0].Rank);
        }

        [TestMethod]
        public void player_should_be_able_to_set_bet()
        {
            var player = new Player("test player");
            
            player.SetBetOnHand(player.Hands[0].Id, 20);

            Assert.AreEqual(20, player.Hands[0].CurrentBet);
        }
        [TestMethod]
        public void player_should_be_able_to_hit_on_active_hand()
        {
            var player = new Player("test player");
            var firstCardInShoe = Game.TheShoe.Cards[0];

            player.HitOnActiveHand();

            Assert.AreEqual(firstCardInShoe.Rank, player.ActiveHand.Cards[0].Rank);
        }

        [TestMethod]
        public void player_should_be_able_to_stand_on_active_hand()
        {
            var player = new Player("test player");

            player.ReceiveCard(AceClub);
            player.StandOnActiveHand();

            Assert.IsTrue(player.ActiveHand.Status == HandStatus.Standing);
        }

        [TestMethod]
        public void player_winning_a_hand_should_add_current_bet_to_bank()
        {
            var player = new Player("test player");
            player.Bank = 100;

            player.SetBetOnHand(player.Hands[0].Id, 100);
            player.WinHand(player.Hands[0].Id);

            Assert.AreEqual(0, player.Hands[0].CurrentBet);
            Assert.AreEqual(200, player.Bank);

        }

        [TestMethod]
        public void player_losing_should_lose_bet()
        {
            var player = new Player("test player");

            player.SetBetOnHand(player.Hands[0].Id, 100);
            player.LoseHand(player.Hands[0].Id);

            Assert.AreEqual(0, player.Hands[0].CurrentBet);
        }

        [TestMethod]
        public void player_should_get_bet_back_on_a_push()
        {
            var player = new Player("test player");
            player.Bank = 100;

            player.SetBetOnHand(player.Hands[0].Id, 20);
            player.PushHand(player.Hands[0].Id);

            Assert.AreEqual(0, player.Hands[0].CurrentBet);
            Assert.AreEqual(100, player.Bank);
        }

        [TestMethod]
        public void player_should_be_able_to_double_bet()
        {
            var player = new Player("test player");
            player.Bank = 100;

            player.SetBetOnHand(player.Hands[0].Id, 20);
            player.DoublingBet(player.Hands[0].Id);

            Assert.AreEqual(40, player.Hands[0].CurrentBet);
            Assert.AreEqual(60, player.Bank);
        }

        [TestMethod]
        public void player_splitting_hand_should_result_in_two_hands()
        {
            var player = new Player("test player");
            player.ReceiveCard(AceClub);
            player.ReceiveCard(AceClub);

            player.SplitHand();

            Assert.AreEqual(2, player.Hands.Count);
        }

        [TestMethod]
        public void player_should_be_able_to_clear_hands()
        {
            var player = new Player("test player");
            player.ReceiveCard(AceClub);
            player.ReceiveCard(AceClub);

            player.SplitHand();
            player.ClearHands();

            Assert.AreEqual(1, player.Hands.Count);
            Assert.AreEqual(0, player.Hands[0].Cards.Count);
        }

    }
}
