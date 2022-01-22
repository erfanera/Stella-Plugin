using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace STAR
{

    public class StellaPluginInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "Stella";
        }
    }
        public class Starcategoryicon :GH_AssemblyPriority
        {
            public override GH_LoadingInstruction PriorityLoad()
            {
                Grasshopper.Instances.ComponentServer.AddCategoryIcon("Stella", Properties.Resources.ATR);
                Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Stella", 'S');

                return GH_LoadingInstruction.Proceed;
            }
        }

        public override Bitmap Icon
    {
        get
        {
            //Return a 24x24 pixel bitmap to represent this GHA library.
            return Properties.Resources.ATR;
        }
    }
    public override string Description
    {
        get
        {
            //Return a short string describing the purpose of this GHA library.
            return "";
        }
    }
    public override Guid Id
    {
        get
        {
            return new Guid("3bf68e89-371f-453b-8717-a6b4e7d9c5ce");
        }
    }

    public override string AuthorName
    {
        get
        {
            //Return a string identifying you or your company.
            return "";
        }
    }
    public override string AuthorContact
    {
        get
        {
            //Return a string representing your preferred contact details.
            return "";
        }
    }
}
}
