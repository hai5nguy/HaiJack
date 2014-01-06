using HaiJack.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaiJack.UnitTest
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void a_jack_face_card_should_return_value_10()
        {
            var card = new Card() {Suit = Suit.Club, Rank = 11};

            var value = card.GetValue();

            Assert.AreEqual(10, value);

        }

        [TestMethod]
        public void a_7_of_diamonds_should_return_value_7()
        {
            var card = new Card() {Suit = Suit.Diamond, Rank = 7};

            var value = card.GetValue();

            Assert.AreEqual(7, value);
        }

        [TestMethod]
        public void an_ace_should_return_value_1()
        {
            var card = new Card() {Suit = Suit.Club, Rank = 1};

            var value = card.GetValue();

            Assert.AreEqual(1, value);
        }


    }
}