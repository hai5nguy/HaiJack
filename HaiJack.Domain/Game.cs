using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HaiJack.Domain
{
    public static class Game
    {
        #region Globals

        public static DealingShoe TheShoe = new DealingShoe();
        public static Dealer TheDealer = new Dealer();
        public static ObservableCollection<Player> ThePlayers = new ObservableCollection<Player>();
        public static GameStatus Status = new GameStatus();
        
        #endregion

        #region Constructors

        static Game()
        {
            Game.Status.InRound = false;
            TheShoe.Shuffle();
        }

        #endregion

        #region Check table

        public static void CheckTable()
        {
            CheckPlayersHands();
            CheckGameProgression();
        }

        private static void CheckPlayersHands(bool finalCheck = false)
        {
            foreach (var player in ThePlayers)
            {
                foreach (var hand in player.Hands)
                {
                    if (finalCheck)
                    {
                        if (hand.IsStanding())
                        {
                            if (hand > TheDealer.Hand) player.WinHand(hand.Id);
                            else if (hand < TheDealer.Hand) player.LoseHand(hand.Id);
                            else player.PushHand(hand.Id);
                        }
                    }
                    else
                    {
                        if (hand.IsBlackjack()) player.WinHand(hand.Id);
                        if (hand.IsTwentyOne()) player.StandHand(hand.Id);
                        if (hand.IsBust()) player.LoseHand(hand.Id);
                        if (hand.IsSplitable()) player.SplitAvailable = true;
                        if (hand.IsValueEleven()) player.DoubleDownAvailable = true;
                    }
                }
            }
        }

        private static void CheckGameProgression()
        {
            if (Status.InRound)
            {
                var playableHandExists = FindPlayableHandAndSetActive();
                if (!playableHandExists)
                {
                    if (StandingHandsExists())
                    {
                        TheDealer.Play();
                        CheckPlayersHands(finalCheck: true);
                    }
                    else
                    {
                        TheDealer.RevealHiddenCard();
                    }
                    Status.InRound = false;
                }
            }
            else if (AllPlayersHaveBets())
            {
                BeginRound();
            }
            //else wait for all players to bet
        }

        private static bool FindPlayableHandAndSetActive()
        {
            bool foundAPlayableHand = false;

            foreach (var player in ThePlayers)
            {
                player.SetActive(false);
                foreach (var hand in player.Hands)
                {
                    hand.SetActive(false);
                    if (!foundAPlayableHand && hand.Status == HandStatus.Playable)
                    {
                        player.SetActive();
                        hand.SetActive();
                        foundAPlayableHand = true;
                    }
                }
            }

            return foundAPlayableHand;
        }

        private static bool StandingHandsExists()
        {
            return (ThePlayers.SelectMany(p => p.Hands).Any(h => h.Status == HandStatus.Standing)) ? true : false;
        }

        #endregion

        #region Between Rounds Methods

        private static bool AllPlayersHaveBets()
        {
            bool betOfZeroFound = false;
            foreach (var player in ThePlayers)
            {
                if (player.Hands[0].CurrentBet == 0) betOfZeroFound = true;
            }

            return (betOfZeroFound == false);

        }

        private static void BeginRound()
        {
            Status.InRound = true;

            ClearCardsOnTable();
            Deal();

            SetAllHandsPlayable();

            CheckTable();
        }

        public static void AddPlayer()
        {
            string newPlayerName = string.Format("Player {0}", ThePlayers.Count + 1);
            ThePlayers.Add(new Player(newPlayerName));
        }

        public static void Deal()
        {
            for (int i = 0; i < 2; i++)
            {
                TheDealer.ReceiveCard(TheShoe.GetCard(hidden: (i == 0)));
                foreach (var player in ThePlayers)
                {
                    player.ReceiveCard(TheShoe.GetCard());
                }
            }
        }

        public static void ClearCardsOnTable()
        {
            TheDealer.ClearHand();

            foreach (var player in ThePlayers)
            {
                player.ClearHands();
            }
        }

        private static void SetAllHandsPlayable()
        {
            foreach (var hand in ThePlayers.SelectMany(p => p.Hands))
            {
                hand.Status = HandStatus.Playable;
            }

        }

        #endregion

        #region Player actions methods

        public static void PlayerBets(Guid playerId, double amount)
        {
            var player = ThePlayers.Single(p => p.Id == playerId);
            player.SetBetOnHand(player.Hands[0].Id, amount);
            CheckTable();
        }

        public static void PlayerHits(Guid playerId)
        {
            var player = ThePlayers.Single(p => p.Id == playerId);
            player.HitOnActiveHand();
            CheckTable();
        }

        public static void PlayerStands(Guid playerId)
        {
            var player = ThePlayers.Single(p => p.Id == playerId);
            player.StandOnActiveHand();
            CheckTable();
        }

        public static void PlayerDoublingDown(Guid playerId)
        {
            var player = ThePlayers.Single(p => p.Id == playerId);
            player.DoublingBet(player.ActiveHand.Id);
            player.HitOnActiveHand();
            player.StandOnActiveHand();
            player.DoubleDownAvailable = false;
            CheckTable();
        }

        public static void PlayerSplitting(Guid playerId)
        {
            var player = ThePlayers.Single(p => p.Id == playerId);
            player.SplitHand();
            CheckTable();
        }

        #endregion

    }
}
