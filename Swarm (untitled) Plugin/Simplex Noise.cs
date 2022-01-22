using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class SimpldexNoise : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public SimpldexNoise()
          : base("Simplex Noise", "Simplex Noise",
              "Create continus noise along the stars ",
              "Stella", "Forces")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Input Position of stars", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Iteration", "i", "Number of iteration", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "s", "Noise factor" ,GH_ParamAccess.item , 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "Output noise vector", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        { 
            List<Point3d> positions = new List<Point3d>();
            DA.GetDataList(0, positions);
            
            int tt = 0;
            DA.GetData(1, ref tt);
            double factor = 1;
            DA.GetData(2, ref factor);
            List<Vector3d> vectors = new List<Vector3d>();
            for(int i = 0; i < positions.Count; i++)
            {
                Point3d spt = positions[i];
                OpenSimplexNoise opt = new OpenSimplexNoise();
                double nx = opt.Evaluate(spt.Y + spt.Z, spt.Y, spt.Z, tt);
                double ny = opt.Evaluate(spt.X, spt.X + spt.Z, spt.Z, tt);
                double nz = opt.Evaluate(spt.X, spt.Y, spt.Y + spt.X, tt);
                vectors.Add(new Vector3d(nx, ny, nz) * factor);

            }

            DA.SetDataList(0, vectors);
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
            get { return new Guid("d914eb6b-aae9-4668-8ff2-c0bc82a39c4f"); }
        }
    }
}