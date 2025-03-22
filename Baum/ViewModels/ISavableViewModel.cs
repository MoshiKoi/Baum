using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baum.ViewModels;

interface ISavableViewModel
{
    IRelayCommand SaveCommand { get; }
}
