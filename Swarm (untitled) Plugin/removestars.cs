using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class removestars : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public removestars()
          : base("Remove", "Remove",
              "Remove Stars if False condition stablished",
              "Stella", "Reproduction")
        {
            
        }
        ///////////choooooooooooopooooooooooooooooooooogh bede bekeshim 
        ////////////chopooogh bazi konim 
        ///choopoogh waschite! elborz! forgive me elborz!

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Star", "SO", "Star object", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Pattern", "P", "If false = remove stars", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Star objects", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<bool> bools = new List<bool>();
            DA.GetDataList(1, bools);
            for(int i = 0; i < part.icle.Count;i++)
            {
                if (bools[i] == false)
                {
                    part.icle.RemoveAt(i);
                }

            }
            DA.SetDataList(0, part.icle);
        } 
        

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f63115fc-2e1c-44e3-83ac-8861badd5660"); }
        }
    }
}