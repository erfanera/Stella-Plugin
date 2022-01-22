using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class StaticEmitter : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public StaticEmitter()
          : base("Point To Star", "Point To Star",
              "Create stars from a list of points",
              "Stella", "Conversion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddPointParameter("Point", "P", "Input points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Life Span", "LS", "Life span of stars", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Reset", "R", "True = reset simulation", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Star Object", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            bool reset = new bool();
            DA.GetData(2, ref reset);
            double lifespan = 12;
            DA.GetData(1, ref lifespan);
            List<Point3d> positions = new List<Point3d>();
            DA.GetDataList(0, positions);
            if (reset)
            {
                part.icle = new List<ParticleAtr>();


                for (int i = 0; i < positions.Count; i++)
                {
                    ParticleAtr pt = new ParticleAtr();
                    pt.History.Add(positions[i]);
                    part.icle.Add(pt);
                    
                }

            }
           // int discpunt = part.icle.Count;
           // if (tt >20)
            //{
                
               // for(int k = 0; k < discpunt; k++)
               // {
               //     ParticleAtr child = new ParticleAtr();
               //     child.History.Add(part.icle[k].History[part.icle[k].History.Count  -2]);
               //     part.icle.Add(child);
               // }
             //   tt = 0;
            //}
           
            for (int j = 0; j <part.icle.Count; j++)
            {
                if (part.icle[j].Age < lifespan)
                {
                    Point3d point = part.icle[j].History[part.icle[j].History.Count - 1];
                    point += part.icle[j].Vel;

                    part.icle[j].History.Add(point);
                    part.icle[j].Age += 1;
                }


                else
                {
                    part.icle.RemoveAt(j);
                }
            }
        
            DA.SetDataList(0,part.icle);

         
     }
        int tt = 0;



                

        

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
            get { return new Guid("49e81eb8-117b-4e4d-af4f-9c7b29c642b2"); }
        }
    }
        }
