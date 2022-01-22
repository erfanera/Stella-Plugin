using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class RandomNoise : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public RandomNoise()
          : base("Random Vector", "Random vector",
              "generate random vector through passing time",
              "Stella", "Forces")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Number of vectors", "N", "Number of random vectors", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Iteration", "i", "Number of iteration", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "s", "Noise factor", GH_ParamAccess.item, 1);
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
            int tt = 0;
            DA.GetData(1, ref tt);
            int number = 0;
            DA.GetData(0, ref number);
            double factor = 0;
            DA.GetData(2, ref factor);
            List<Vector3d> vectors = new List<Vector3d>();

            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                
                vectors.Add(new Vector3d((random.NextDouble() * 2) - 1, (random.NextDouble() * 2) - 1, (random.NextDouble() * 2) - 1) *factor);
            }
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
            get { return new Guid("13b8735c-4acb-4a13-bef1-3f5e8d5d0a2b"); }
        }
    }
}