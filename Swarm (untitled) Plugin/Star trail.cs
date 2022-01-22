using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class StarLifeghc : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public StarLifeghc()
          : base("Star Trail", "Star Trail",
              "Show stars' trail as curves",
              "Stella", "Conversion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Star", "SO", "input star output", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "show path", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<ParticleAtr> part = new List<ParticleAtr>();
            DA.GetDataList(0, part);
            
            List<Polyline> curves = new List<Polyline>();
            for (int i = 0; i < part.Count; i++)
            {
                Polyline pl = new Polyline();
                for(int j = 0; j < part[i].History.Count;j++)
                {
                    pl.Add(part[i].History[j]);
                }

                curves.Add(pl);

            }
            DA.SetDataList(0, curves);



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
                return Properties.Resources.TRAIL;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8895c87a-c085-4d90-8b56-0f1b6437d268"); }
        }
    }
}