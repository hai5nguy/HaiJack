using System;
using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaiJack.UnitTest
{
    [TestClass]
    public class DealingShoeTests
    {
        [TestMethod]
        public void shuffling_dealing_shoe_should_return_the_same_number_of_cards()
        {
            var shoe = new DealingShoe();

            var before = shoe.Cards.Count;

            shoe.Shuffle();

            var after = shoe.Cards.Count;

            Assert.AreEqual(before, after);
        }

        [TestMethod]
        public void getting_a_card_from_shoe_returns_the_first_card()
        {
            var shoe = new DealingShoe();

            var firstCard = shoe.Cards[0];

            var dealtCard = shoe.GetCard();

            Assert.AreEqual(firstCard, dealtCard);
        }

        [TestMethod]
        public void getting_a_card_from_the_dealing_shoe_removes_it()
        {
            var shoe = new DealingShoe();

            var dealtCard = shoe.GetCard();

            CollectionAssert.DoesNotContain(shoe.Cards, dealtCard);
        }
    }
}
