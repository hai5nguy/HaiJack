using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaiJack.Domain
{
    public class Card : NotifyPropertyChangeBase
    {
        private const string CARD_BACK_IMAGE = @"..\Images\card-back-blue.png";

        private string _imageUri;
        private bool _hidden;


        public Suit Suit { get; set; }
        public int Rank { get; set; }

        public bool Hidden
        {
            get { return _hidden; }
            set
            {
                _hidden = value;
                NotifyPropertyChange("ImageUri");
            }
        }

        public string ImageUri
        {
            get { return Hidden ? CARD_BACK_IMAGE : _imageUri; }
            set { _imageUri = value; }
        }

        private string _imageMargin;

        public string ImageMargin
        {
            get { return _imageMargin; }
            set
            {
                _imageMargin = value;
                NotifyPropertyChange("ImageMargin");
            }
        }

        public int GetValue()
        {
            return (Rank > 10) ? 10 : Rank;
        }

    }

    public enum Suit
    {
        Club, Diamond, Heart, Spade
    }

}
