using System;
using System.Windows.Input;

namespace GalvanizedSoftware.Beethoven.DemoApp.Common
{
  public class Command : ICommand
  {
    private readonly Action action;
    private bool canExecute = true;

    public Command(Action action)
    {
      this.action = action;
    }

    public bool CanExecute(object parameter)
    {
      return canExecute;
    }

    public void SetCanExecute(bool canExecuteNow)
    {
      canExecute = canExecuteNow;
      CanExecuteChanged(this, new EventArgs());
    }

    public void Execute(object parameter)
    {
      action();
    }

    public event EventHandler CanExecuteChanged = delegate { };
  }
}