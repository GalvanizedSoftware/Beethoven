using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GalvanizedSoftware.Beethoven.DemoApp.Extending
{
  internal class ApprovalAccount : INotifyPropertyChanged
  {
    private readonly List<double> approved = new List<double>();

    public ApprovalAccount(string name)
    {
      Name = name;
    }

    internal void Approve(double amount)
    {
      approved.Add(amount);
      OnPropertyChanged(nameof(Total));
      OnPropertyChanged(nameof(Average));
    }

    public double Total => approved.Sum();

    public double Average => Total / approved.Count;

    public string Name { get; }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
