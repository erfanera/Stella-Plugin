using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class StarBoundry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public StarBoundry()
          : base("Star Collide", "Star Collide",
              "Create collision between stars",
              "Stella", "Forces")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Input points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radius", "r", "Boundry radius", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "Output vectors", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(0, points);
            double radius = 0;
            DA.GetData(1, ref radius);
            List<int> count = new List<int>();
            List<Vector3d> vect = new List<Vector3d>();
            for(int i = 0; i < points.Count; i++)
            {
                count.Add(0);
                vect.Add(new Vector3d(0, 0, 0));

            }
            for(int j = 0; j < points.Count; j++)
            {
                for(int k = j + 1; k < points.Count; k++)
                {
                    double d = points[j].DistanceTo(points[k]);
                    if (d < radius * 2)
                    {
                        count[j] += 1;
                        count[k] +=1;
                       Vector3d vector = points[k] - points[j];
                        vector.Unitize();
                        double factor = (d - (radius * 2) )* 0.5;
                        vector *= factor;
                        vect[j] += vector;
                        vect[k] -= vector;



                    }
                }
            }
            DA.SetDataList(0, vect);

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
            get { return new Guid("2fd60601-4076-4df0-9ee7-ffa1e202bc36"); }
        }
    }
}