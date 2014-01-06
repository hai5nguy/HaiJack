using System;
using System.Linq;
using System.Threading;
using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaiJack.UnitTest
{
    [TestClass]
    public class GameTests
    {
        private Card AceClub
        {
            get
            {
                return new Card() { Rank = 1, Suit = Suit.Club };
            }
        }

        private Card JackClub
        {
            get { return new Card() { Rank = 11, Suit = Suit.Club }; }
        }

        private Card ThreeClub { get { return new Card() {Rank = 3, Suit = Suit.Club}; }}
        private Card EightClub { get { return new Card() {Rank = 8, Suit = Suit.Club}; }}

        private Hand HandWith20()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Rank = 10, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 10, Suit = Suit.Club });
            return hand;
        }

        private Hand HandWith21()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Rank = 11, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 1, Suit = Suit.Club });
            return hand;
        }

        private Hand HandWithBust()
        {
            var hand = new Hand();
            hand.AddCard(new Card() { Rank = 10, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 10, Suit = Suit.Club });
            hand.AddCard(new Card() { Rank = 2, Suit = Suit.Club });
            return hand;
        }

        private void SetupDealerWith20()
        {
            Game.TheDealer.Hand.AddCard(new Card() { Rank = 10, Suit = Suit.Spade });
            Game.TheDealer.Hand.AddCard(new Card() { Rank = 10, Suit = Suit.Spade });
        }

        private Player SetupPlayerWith21()
        {
            var player = new Player("Test");
            player.ActiveHand.AddCard(new Card() { Rank = 11, Suit = Suit.Club });
            player.ActiveHand.AddCard(new Card() { Rank = 1, Suit = Suit.Club });
            return player;
        }

        private void SetupActivePlayer()
        {
            Game.ThePlayers.Clear();
            Game.AddPlayer();
            Game.ThePlayers[0].SetActive();
        }

        [TestMethod]
        public void game_table_should_setup_dealer()
        {
            Assert.IsNotNull(Game.TheDealer);
        }

        [TestMethod]
        public void game_table_should_setup_list_of_players()
        {
            Assert.IsNotNull(Game.ThePlayers);
        }

        [TestMethod]
        public void the_game_table_should_allow_a_player_to_make_bets()
        {

            Game.AddPlayer();

            var firstPlayer = Game.ThePlayers[0];

            Game.PlayerBets(firstPlayer.Id, 20);

            Assert.AreEqual(20, firstPlayer.ActiveHand.CurrentBet);
        }

        [TestMethod]
        public void game_should_allow_player_to_hit()
        {
            SetupActivePlayer();


            var player = Game.ThePlayers[0];

            var firstCardOfShoe = Game.TheShoe.Cards[0];

            Game.PlayerHits(player.Id);

            var lastCardReceive = player.Hands[0].Cards[0];

            Assert.AreEqual(firstCardOfShoe, lastCardReceive);

        }

        [TestMethod]
        public void game_should_advance_to_dealer_when_player_hit_21()
        {
            SetupActivePlayer();
            SetupDealerWith20();

            Game.TheShoe.Cards[0] = AceClub;
            Game.TheShoe.Cards[1] = JackClub;

            var player = Game.ThePlayers[0];
            player.Hands[0].Status = HandStatus.Playable;

            Game.PlayerHits(player.Id);
            Game.PlayerHits(player.Id);

            Assert.IsTrue(player.ActiveHand.Status == HandStatus.Finished);
            Assert.IsTrue(player.ActiveHand.ShowWonMessage);
        }

        [TestMethod]
        public void game_should_adance_to_next_hand_when_player_hit_21() 
        {
            SetupActivePlayer();
            var player = Game.ThePlayers[0];
            player.Hands.Add(new Hand());

            Game.TheShoe.Cards[0] = JackClub;
            Game.TheShoe.Cards[1] = JackClub;
            Game.TheShoe.Cards[2] = AceClub;
            Game.Status.InRound = true;

            player.Hands[0].Status = HandStatus.Playable;
            player.Hands[1].Status = HandStatus.Playable;

            Game.PlayerHits(player.Id);
            Game.PlayerHits(player.Id);
            Game.PlayerHits(player.Id);

            Assert.IsTrue(player.Hands[1].Active);
        }
        [TestMethod]
        public void game_should_advance_to_next_player_when_player_hit_21()
        {
            SetupActivePlayer();
            SetupDealerWith20();

            Game.AddPlayer();
            var player1 = Game.ThePlayers[0];
            var player2 = Game.ThePlayers[1];

            Game.TheShoe.Cards[0] = JackClub;
            Game.TheShoe.Cards[1] = JackClub;
            Game.TheShoe.Cards[2] = AceClub;
            Game.Status.InRound = true;

            player1.Hands[0].Status = HandStatus.Playable;
            player2.Hands[0].Status = HandStatus.Playable;

            Game.PlayerHits(player1.Id);
            Game.PlayerHits(player1.Id);
            Game.PlayerHits(player1.Id);

            Assert.IsFalse(Game.ThePlayers[0].Active);
            Assert.IsTrue(Game.ThePlayers[1].Active);
        }


        [TestMethod]
        public void game_should_advance_to_dealer_when_player_stand()
        {
            Game.Status.InRound = true;

            SetupActivePlayer();
            SetupDealerWith20();

            var player = Game.ThePlayers[0];
            player.Hands[0].Status = HandStatus.Playable;

            player.ReceiveCard(JackClub);
            player.ReceiveCard(JackClub);

            Game.PlayerStands(player.Id);

            Assert.IsTrue(player.Hands[0].Status == HandStatus.Finished);
        }
        [TestMethod]
        public void game_should_advance_to_next_hand_when_player_stands()
        {
            Game.Status.InRound = true;
            SetupActivePlayer();
            var player = Game.ThePlayers[0];
            player.Hands.Add(new Hand());

           

            player.Hands[0].AddCard(JackClub);
            player.Hands[0].AddCard(JackClub);

            player.Hands[0].Status = HandStatus.Playable;
            player.Hands[1].Status = HandStatus.Playable;

            Game.PlayerStands(player.Id);

            Assert.IsFalse(player.Hands[0].Active);
            Assert.IsTrue(player.Hands[1].Active);
        }

        [TestMethod]
        public void game_should_advance_to_next_player_when_player_stands()
        {
            Game.Status.InRound = true;
            SetupActivePlayer();
            SetupDealerWith20();

            Game.AddPlayer();
            var player1 = Game.ThePlayers[0];
            var player2 = Game.ThePlayers[1];

            Game.TheShoe.Cards[0] = JackClub;
            Game.TheShoe.Cards[1] = JackClub;
            Game.TheShoe.Cards[2] = AceClub;


            player1.Hands[0].Status = HandStatus.Playable;
            player2.Hands[0].Status = HandStatus.Playable;

            player1.Hands[0].AddCard(JackClub);
            player1.Hands[0].AddCard(JackClub);

            Game.PlayerStands(player1.Id);


            Assert.IsFalse(Game.ThePlayers[0].Active);
            Assert.IsTrue(Game.ThePlayers[1].Active);
        }

        [TestMethod]
        public void game_should_advance_to_next_player_when_player_double_down()
        {
            Game.Status.InRound = true;
            SetupActivePlayer();
            SetupDealerWith20();

            Game.AddPlayer();
            var player1 = Game.ThePlayers[0];
            var player2 = Game.ThePlayers[1];

            Game.TheShoe.Cards[0] = JackClub;


            player1.Hands[0].Status = HandStatus.Playable;
            player2.Hands[0].Status = HandStatus.Playable;

            player1.Hands[0].AddCard(ThreeClub);
            player1.Hands[0].AddCard(EightClub);

            Game.PlayerDoublingDown(player1.Id);

            Assert.IsFalse(Game.ThePlayers[0].Active);
            Assert.IsTrue(Game.ThePlayers[1].Active);
        }

        [TestMethod]
        public void game_should_set_action_on_first_hand_after_player_splits()
        {
            Game.Status.InRound = true;
            SetupActivePlayer();
            SetupDealerWith20();

            var player = Game.ThePlayers[0];


            player.Hands[0].Status = HandStatus.Playable;

            player.Hands[0].AddCard(JackClub);
            player.Hands[0].AddCard(JackClub);

            Game.PlayerSplitting(player.Id);

            Assert.IsTrue(player.Hands[0].Active);
            Assert.IsFalse(player.Hands[1].Active);
        }
        

        //[TestMethod]
        //public void game_should_bust_player_who_hit_over_21()
        //{
        //    SetupActivePlayer();
        //    SetupDealerWith20();

        //    var player = Game.ThePlayers[0];

        //    Game.TheShoe.Cards[0] = Game.TheShoe.Cards.Find(c => c.Rank == 11 && c.Suit == Suit.Club);
        //    Game.TheShoe.Cards[1] = Game.TheShoe.Cards.Find(c => c.Rank == 11 && c.Suit == Suit.Diamond);
        //    Game.TheShoe.Cards[2] = Game.TheShoe.Cards.Find(c => c.Rank == 11 && c.Suit == Suit.Spade);

        //    Game.PlayerHits(player.Id);
        //    Game.PlayerHits(player.Id);
        //    Game.PlayerHits(player.Id);

        //    Assert.IsTrue(player.Hand.Status == HandStatus.Finished);
        //}

        //[TestMethod]
        //public void game_should_stand_when_player_hit_for_21()
        //{
        //    SetupActivePlayer();
        //    SetupDealerWith20();

        //    var player = Game.ThePlayers[0];

        //    Game.TheShoe.Cards[0] = Game.TheShoe.Cards.Find(c => c.Rank == 3 && c.Suit == Suit.Club);
        //    Game.TheShoe.Cards[1] = Game.TheShoe.Cards.Find(c => c.Rank == 8 && c.Suit == Suit.Diamond);
        //    Game.TheShoe.Cards[2] = Game.TheShoe.Cards.Find(c => c.Rank == 11 && c.Suit == Suit.Spade);

        //    Game.PlayerHits(player.Id);
        //    Game.PlayerHits(player.Id);
        //    Game.PlayerHits(player.Id);

        //    Assert.IsTrue(player.Hand.Stand);
        //}

        //[TestMethod]
        //public void game_should_allow_player_to_stand()
        //{
        //    SetupActivePlayer();
        //    SetupDealerWith20();

        //    var player = Game.ThePlayers[0];

        //    Game.TheShoe.Cards[0] = Game.TheShoe.Cards.Find(c => c.Rank == 10 && c.Suit == Suit.Club);
        //    Game.TheShoe.Cards[1] = Game.TheShoe.Cards.Find(c => c.Rank == 10 && c.Suit == Suit.Diamond);

        //    player.ReceiveCard(Game.TheShoe.GetCard());
        //    player.ReceiveCard(Game.TheShoe.GetCard());

        //    Game.PlayerStands(player.Id);

        //    Assert.IsTrue(player.Hand.Stand);
        //}

        //[TestMethod]
        //public void game_should_allow_player_to_double_down()
        //{
        //    SetupActivePlayer();
        //    SetupDealerWith20();

        //    var player = Game.ThePlayers[0];

        //    Game.TheShoe.Cards[0] = Game.TheShoe.Cards.Find(c => c.Rank == 3 && c.Suit == Suit.Club);
        //    Game.TheShoe.Cards[1] = Game.TheShoe.Cards.Find(c => c.Rank == 8 && c.Suit == Suit.Diamond);

        //    player.ReceiveCard(Game.TheShoe.GetCard());
        //    player.ReceiveCard(Game.TheShoe.GetCard());

        //    Game.PlayerDoublingDown(player.Id);

        //    Assert.AreEqual(3, player.Hand.Cards.Count);
        //    Assert.IsTrue(player.Hand.Stand);

        //}

        //[TestMethod]
        //public void game_should_allow_player_to_split_hand()
        //{
        //    SetupActivePlayer();
        //    SetupDealerWith20();

        //    var player = Game.ThePlayers[0];

        //    Game.TheShoe.Cards[0] = Game.TheShoe.Cards.Find(c => c.Rank == 8 && c.Suit == Suit.Club);
        //    Game.TheShoe.Cards[1] = Game.TheShoe.Cards.Find(c => c.Rank == 8 && c.Suit == Suit.Diamond);

        //    player.ReceiveCard(Game.TheShoe.GetCard());
        //    player.ReceiveCard(Game.TheShoe.GetCard());

        //    Game.PlayerSplitting(player.Id);

        //    Assert.AreEqual(2, player.Hands.Count);
        //    Assert.AreEqual(2, player.Hands[0].Cards.Count);
        //    Assert.AreEqual(2, player.Hands[1].Cards.Count);
        //}


        //[TestMethod]
        //public void the_game_should_set_win_for_player_with_21_vs_dealer_with_20()
        //{
        //    Game.TheDealer.Active = HandWith20();
        //    Game.ThePlayers[0].Active = HandWith21();

        //    Game.CheckTable();

        //    Assert.IsTrue(Game.ThePlayers[0].Active.ShowWonMessage);
        //}

        //[TestMethod]
        //public void the_game_should_set_lost_for_player_with_20_vs_dealer_with_21()
        //{
        //    Game.TheDealer.Active = HandWith21();
        //    Game.ThePlayers[0].Active = HandWith20();

        //    Game.CheckTable();

        //    Assert.IsTrue(Game.ThePlayers[0].Active.ShowLostMessage);
        //}


        //[TestMethod]
        //public void game_start_should_deal_two_cards_two_each_players()
        //{
        //    Game.Start();

        //    foreach (var player in Game.ThePlayers)
        //    {
        //        Assert.AreEqual(2, player.Hands.First().Cards.Count);
        //    }
        //}
    }
}
