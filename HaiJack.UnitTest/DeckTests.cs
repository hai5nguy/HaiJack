using System;
using System.Linq;
using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaiJack.UnitTest
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void an_ace_of_club_should_have_image_uri_1()
        {
            var deck = new Deck();

            var aceOfClub = deck.Cards.Where(c => c.Suit == Suit.Club && c.Rank == 1).First();

            Assert.AreEqual(@"..\Images\1.png", aceOfClub.ImageUri);
        }
        [TestMethod]
        public void an_ace_of_diamond_should_have_image_uri_14()
        {
            var deck = new Deck();

            var aceOfClub = deck.Cards.Where(c => c.Suit == Suit.Diamond && c.Rank == 1).First();

            Assert.AreEqual(@"..\Images\14.png", aceOfClub.ImageUri);
        }
        [TestMethod]
        public void an_ace_of_heart_should_have_image_uri_27()
        {
            var deck = new Deck();

            var aceOfClub = deck.Cards.Where(c => c.Suit == Suit.Heart && c.Rank == 1).First();

            Assert.AreEqual(@"..\Images\27.png", aceOfClub.ImageUri);
        }
        [TestMethod]
        public void an_ace_of_spade_should_have_image_uri_40()
        {
            var deck = new Deck();

            var aceOfClub = deck.Cards.Where(c => c.Suit == Suit.Spade && c.Rank == 1).First();

            Assert.AreEqual(@"..\Images\40.png", aceOfClub.ImageUri);
        }
    }
}
