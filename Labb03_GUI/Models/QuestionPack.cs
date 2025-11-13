namespace Labb03_GUI.Models
{
    internal enum Difficulty { Easy, Medium, Hard }
    internal class QuestionPack
    {
        public string Name { get; set; }
        public Array Difficulties => Enum.GetValues(typeof(Difficulty));
        public Difficulty Difficulty { get; set; }
        public List<Question> Questions { get; set; }
        public int TimeLimitInSeconds { get; set; }

        public QuestionPack(string name = "MyQuestionPack", Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Question>();
        }
    }
}
