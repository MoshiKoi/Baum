using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baum.ViewModels;

public partial class LanguageItemViewModel : ViewModelBase
{
    public required int LanguageId { get; set; }
    public required string Name { get; set; }
    public required ProjectForestViewModel Parent { get; set; }


    [RelayCommand]
    public void OpenLanguage()
    {
        Parent.OpenLanguage(LanguageId);
    }
}
