using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.ViewModels
{
    class AnswerViewModel : ViewModelBase
    {
        public string Text { get; }

        private bool? _isCorrect;
        public bool? IsCorrect
        {
            get => _isCorrect;
            set { _isCorrect = value; RaisePropertyChanged(); }
        }

        public AnswerViewModel(string text)
        {
            Text = text;
            _isCorrect = null;
        }
    }
}
