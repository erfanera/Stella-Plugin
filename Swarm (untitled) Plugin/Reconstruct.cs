using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class additionalapplier : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public additionalapplier()
          : base("Reconstruct", "ReCons",
              "Use with Deconstruct component to make a loop. Replace new position and velocity to Star",
              "Stella", "Loop")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)

        {
            pManager.AddPointParameter("Point", "P", "Input Points", GH_ParamAccess.list);
            pManager.AddVectorParameter("Vector", "V", "Input Vectors", GH_ParamAccess.list);
          
            //pManager.AddGenericParameter("stars", "s", "s", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Star Objects", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Vector3d> vectors = new List<Vector3d>();
            DA.GetDataList(1, vectors);
            List<Point3d> pt = new List<Point3d>();
            DA.GetDataList(0, pt);
            for(int i = 0; i < part.icle.Count; i++)
            {
                part.icle[i].History.Add(pt[i]);
                part.icle[i].Vel = vectors[i];
            }
            
            DA.SetData(0,part.icle);
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
            get { return new Guid("6d6405bd-b713-4bb8-87f9-03c95fd17b81"); }
        }
    }
}