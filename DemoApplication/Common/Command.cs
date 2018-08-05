using System;
using System.Windows.Input;

namespace GalvanizedSoftware.Beethoven.DemoApp.Common
{
  public class Command : ICommand
  {
    private readonly Action action;

    public Command(Action action)
    {
      this.action = action;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      action();
    }

    public event EventHandler CanExecuteChanged;
  }
}