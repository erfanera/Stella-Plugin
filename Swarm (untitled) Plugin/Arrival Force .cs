using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class ArrivalForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public ArrivalForce ()
          : base("Arrival Force", "Arrival Force",
              "Calculate force to achieve the target and slow down the velocity when they are in the defined radius.",
              "Stella", "Forces")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Input position of stars", GH_ParamAccess.list);
            pManager.AddPointParameter("Targets", "T" ,"Targets to reach(if you are defining a list of target each star react to the closest target)" ,GH_ParamAccess.list);
            pManager.AddNumberParameter("Arrival Raduis", "r", "Radius which force stars to slow down their velocity to target", GH_ParamAccess.item,1);
            pManager.AddNumberParameter("Factor", "F", "Force factor", GH_ParamAccess.item ,0.2);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "Final arrival vector", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double departure = 2.0;
            DA.GetData(2, ref departure);
            List<Point3d> positions = new List<Point3d>();
            List<Point3d> Targets = new List<Point3d>();
            DA.GetDataList(0, positions);
            DA.GetDataList(1, Targets);
            double factor = 0;
            DA.GetData(3, ref factor);
            List<Vector3d> vects = new List<Vector3d>();
            for (int i = 0; i < positions.Count; i++)
            {
                double distance = 0;
                Vector3d force = new Vector3d();
                if (Targets.Count == 1)
                {
                    force =(Targets[0] - positions[i]) * factor;
                    distance = positions[i].DistanceTo(Targets[0]);
                }
                else
                {
                   Point3d closestTarget = new Point3d(1000000, 1000000, 1000000);
                    for (int j = 0; j < Targets.Count; j++)
                    {

                        if (positions[i].DistanceTo(Targets[j]) < positions[i].DistanceTo(closestTarget))
                        {
                            closestTarget = Targets[j];
                            
                        }
                    }



                    distance = positions[i].DistanceTo(closestTarget);
                   force =(closestTarget - positions[i]) * factor;
                }
                if (distance< departure)
                {
                    // distance = departure  or distance=departure factor==> 1
                    // distance< departure    factor==> <1      (
                    double persenatge = distance / departure;
                    force *= persenatge;


                }
                vects.Add(force);

            }
            DA.SetDataList(0, vects);
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
            get { return new Guid("76bc72f4-6425-4302-a97a-23b528dc85e4"); }
        }
    }
}