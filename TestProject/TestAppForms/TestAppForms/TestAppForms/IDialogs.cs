using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppForms
{
  public interface IDialogs
  {
    void DisplayActionSheet(string title, string positiveText, string[] items, Action<int> callback);
		
  }
}
