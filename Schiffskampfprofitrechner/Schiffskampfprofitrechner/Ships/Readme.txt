This folder is cleared of any present .cs files whenever shipGenList.py is run.
Until the python tool is run, any .cs classes may be edited freely, however progress will be lost when the tool is run again next.

Standard file structure:

using System;

namespace SKPR.Ships
{ 
    public class YWing
    {
        string Name { get; set; }
        int[] Resources { get; set; }
        string Faction { get; set; }
    }
}
