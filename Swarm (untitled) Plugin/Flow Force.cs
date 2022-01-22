using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class FlowForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the createobstacle class.
        /// </summary>
        public FlowForce()
          : base("Flow Force", "Flow Force",
              "Create gliding vector over nearest surface",
              "Stella", "Forces")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Connect particles last position", GH_ParamAccess.list);
            pManager.AddVectorParameter("Velocity", "Vel", "Connect current velocity of particles", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("Surface", "S", "Surface to gliding over", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "T", "In case stars still doesn't react to surfaces, increase this value", GH_ParamAccess.item,0);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Velocity", "Vel", "Final velocity (it is neccessary to be the last operation on vectors.)", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //GENERATING PARAMETERS--------------------------
            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(0, points);
            List<Vector3d> vects = new List<Vector3d>();
            List<Vector3d> vectcopy = new List<Vector3d>();
            DA.GetDataList(1, vects);
            DA.GetDataList(1, vectcopy);
            List<Surface> breps = new List<Surface>();
            DA.GetDataList(2, breps);
            double tol = 0;
            DA.GetData(3, ref tol);
            //INITIATION PROCESS-------------------
            List<Vector3d> finalvects = new List<Vector3d>();
            List<Vector3d> inverseforce = new List<Vector3d>(); 

            for(int i = 0; i < points.Count; i++)
            { 
               
                vectcopy.Add(vects[i]);
            }
            

            //Main looop
            for (int j = 0; j < points.Count; j++)
            {
                
                //initiation data to loop on!
                //double tu;   //local
                //double tv;   //local
                // Point3d tposition;     //to replace
                //Vector3d[] tvectorxy;  //to replace
                // breps[0].ClosestPoint(points[j], out tu, out tv);
                //breps[0].Evaluate(tu, tv, 0, out tposition, out tvectorxy);

                Point3d closestpoint = new Point3d(100000, 100000, 100000);   // tposition;     //global
         

                Point3d position;
                Vector3d[] vectorxy;
                //evaluate all surfaces and find closest surface
                
                Vector3d formal = new Vector3d(0, 0, 0);
                for (int k = 0; k < breps.Count; k++)
                {
                    double u;
                    double v;
                    
                    breps[k].ClosestPoint(points[j], out u, out v);
                    breps[k].Evaluate(u, v, 1, out position, out vectorxy);
                    Vector3d normal = breps[k].NormalAt(u, v);
                    

                    if (position.DistanceTo(points[j]) < closestpoint.DistanceTo(points[j]))
                    {
                        closestpoint = position;
                        formal = normal;
                        
                        
                    }
                }
                double finaldistance = points[j].DistanceTo(closestpoint);
                if (finaldistance < vectcopy[j].Length+tol)
                {

                    Vector3d invers = vects[j];
                    invers += points[j] - closestpoint;
                    Transform mirror = Transform.Mirror(closestpoint, formal);
                    invers.Transform(mirror);
                    inverseforce.Add(invers);
                }
                else { inverseforce.Add(vectcopy[j]);}

                    // Transform invers = Transform.Translation(inverseforce[j]);
        


                     
                finalvects.Add((inverseforce[j]+vects[j])*0.5);
            }
            DA.SetDataList(0, finalvects);
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
            get { return new Guid("ad99c3b9-9288-405d-8c1f-ed9ddb14af9f"); }
        }
    }
}