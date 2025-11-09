using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.Models
{
    internal class ImportQuestion
    {
		private int _numbersOfQuestions;
        public Difficulty Difficulty { get; set; }

        public int NumberOfQuestions
		{
			get => _numbersOfQuestions;
			set 
			{
				if (value < 1) _numbersOfQuestions = 1;
				else if (value > 20) _numbersOfQuestions = 20;
				else _numbersOfQuestions = value;
            }
		}

	}
}
