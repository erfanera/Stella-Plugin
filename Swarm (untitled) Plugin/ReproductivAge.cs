using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class ReproductivAge : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReproductivAge class.
        /// </summary>
        public ReproductivAge()
          : base("Reproductive Age", "Reproductive Age",
              "Stars reproduction based on parents reproductive age",
              "Stella", "Reproduction")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Stars to reproduct from", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Reproductive Age", "Re.Age", "Age of reproduction, in case you don't want any limit set it to 0", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Number Of Child", "No.Child", "Number of child", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Childs LifeSpan", "Lf.Child", "Childs life span", GH_ParamAccess.item);
            pManager.AddNumberParameter("Reproduction Radius", "Re.Radius", "Boundry radius which stars allowed to reproduct", GH_ParamAccess.item,1);
          //pManager.AddBooleanParameter("Kill", "Kill", "If True Parents will killed", GH_ParamAccess.item);
            

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
            List<int> reproductionage = new List<int>();
            DA.GetDataList(1, reproductionage);
            int NoChild = 0;
            DA.GetData(2, ref NoChild);
            int ChildLF = 0;
            DA.GetData(3, ref ChildLF);
            double maxDist = 0;
            DA.GetData(4, ref maxDist);
            bool kill = false;
           // DA.GetData(5, ref kill);
            List<ParticleAtr> newlist = new List<ParticleAtr>();
            List<int> index = new List<int>();
            //------------------------------------------
            int count = part.icle.Count - 1;
            
            for (int i = 0; i < count; i++)
            {
                
                 double d = part.icle[i].History[part.icle[i].History.Count - 1].DistanceTo(part.icle[i + 1].History[part.icle[i + 1].History.Count - 1]);
                if (d > maxDist * 2 - 0.1 && reproductionage[0] == 0)
                {

                    Point3d avr = (part.icle[i].History[part.icle[i].History.Count - 1] + part.icle[i + 1].History[part.icle[i + 1].History.Count - 1]) * 0.5;
                    for (int k = 0; k < NoChild; k++)
                    {
                        ParticleAtr child = new ParticleAtr();
                        child.History.Add(avr);
                        child.LifeSpan = ChildLF;
                        newlist.Add(child);
                        index.Add(i + 1 + index.Count);

                    }
                }
                            
                    
                else
                {


                    if (part.icle[i].Age == reproductionage[0])
                    {
                        reproductionage.RemoveAt(0);
                        if (d > maxDist * 2 - 0.1)

                        {
                            Point3d avr = (part.icle[i].History[part.icle[i].History.Count - 1] + part.icle[i + 1].History[part.icle[i + 1].History.Count - 1]) * 0.5;
                            for (int k = 0; k < NoChild; k++)
                            {
                                ParticleAtr child = new ParticleAtr();
                                child.History.Add(avr);
                                child.LifeSpan = ChildLF;
                                newlist.Add(child);
                                index.Add(i + 1 + index.Count);
                            }

                        }
                    }

                    
                }
                
            }
                for (int t = 0; t < index.Count; t++)
                {
                    part.icle.Insert(index[t], newlist[t]);
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
            get { return new Guid("a852b39a-5320-4054-bc13-6b199ede9fa4"); }
        }
    }
}