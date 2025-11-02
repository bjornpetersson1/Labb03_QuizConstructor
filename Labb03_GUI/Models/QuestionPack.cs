namespace Labb03_GUI.Models
{
    internal enum Difficulty { Easy, Medium, Hard }
    internal class QuestionPack
    {
        private int _timeLimitInSeconds;
        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Question>();
        }
        public Array Difficulties => Enum.GetValues(typeof(Difficulty));
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public int TimeLimitInSeconds 
        {
            get => _timeLimitInSeconds;
            set
            {
                if (value < 5) _timeLimitInSeconds = 5;
                else if (value > 120) _timeLimitInSeconds = 120;
                else
                {
                    int rest = value % 5;
                    if (rest <= 2) _timeLimitInSeconds = value - rest;
                    else _timeLimitInSeconds = value + 5 - rest;
                }
            }
        }
        public List<Question> Questions { get; set; }
    }
}
