using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace M2ViewModelLib.ViewModels.Basic
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _onExecute;
        private readonly Action _onExecuteNoParam;
		private ICommand saveSelectedWS;

		/// <summary>Событие извещающее об измении состояния команды</summary>
		public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>Конструктор команды</summary>
        /// <param name="execute">Выполняемый метод команды</param>
        /// <param name="canExecute">Метод разрешающий выполнение команды</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _onExecute = execute;
            _canExecute = canExecute;
        }
        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            _onExecuteNoParam = execute;
            _canExecute = canExecute;
        }

		public RelayCommand(ICommand saveSelectedWS)
		{
			this.saveSelectedWS = saveSelectedWS;
		}

		/// <summary>Вызов разрешающего метода команды</summary>
		/// <param name="parameter">Параметр команды</param>
		/// <returns>True - если выполнение команды разрешено</returns>
		public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute.Invoke(parameter);
        /// <summary>Вызов выполняющего метода команды</summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter)
        {
            _onExecuteNoParam?.Invoke();
            _onExecute?.Invoke(parameter);
        }
    }
}
