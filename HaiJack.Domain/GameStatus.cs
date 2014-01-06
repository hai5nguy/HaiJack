namespace HaiJack.Domain
{
    public class GameStatus : NotifyPropertyChangeBase
    {
        private bool _inRound;
        public bool InRound
        {
            get { return _inRound; }
            set
            {
                _inRound = value;
                NotifyPropertyChange("InRound");
                InBetweenRounds = !value;
            }
        }

        private bool _inBetweenRounds = true;
        public bool InBetweenRounds
        {
            get { return _inBetweenRounds; }
            set
            {
                _inBetweenRounds = value;
                NotifyPropertyChange("InBetweenRounds");
            }
        }
    }
}