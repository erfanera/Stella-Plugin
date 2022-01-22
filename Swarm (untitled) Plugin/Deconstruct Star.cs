using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class testdecons : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public testdecons()
          : base("Deconstruct Stars", "Deconstruct Stars",
              "Use with Reconstruct component to make a loop.",
              "Stella", "Loop")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Star object", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Output points", GH_ParamAccess.list);
            pManager.AddVectorParameter("Vector", "V", "Output Vectors", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
           List<ParticleAtr> pt = new List<ParticleAtr>();
            DA.GetDataList(0, pt);
            List<Point3d> point = new List<Point3d>();
            List<Vector3d> vecs = new List<Vector3d>();
            for (int i = 0; i < pt.Count; i++)
            {
                point.Add(pt[i].History[pt[i].History.Count - 1]);
                vecs.Add(pt[i].Vel);

            }
            DA.SetDataList(0, point);
            DA.SetDataList(1, vecs);



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
            get { return new Guid("7a56abff-bb6d-4a39-95a9-213ead7db3c5"); }
        }
    }
}