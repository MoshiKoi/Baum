using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baum.DB;

public class SoundChange
{
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public Language Language { get; set; }
    public required string Notation { get; set; }
}
