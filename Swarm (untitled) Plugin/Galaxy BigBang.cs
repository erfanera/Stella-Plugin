using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace STAR
{
    public class GBB : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GBB()
          : base("BigBang", "BigBang",
              "BigBang is a solver which simulate the act of stars based on input parameters.",
              "Stella", "Conversion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Environment Boundries", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Noise", "N", " 0=None , 1=Random ,2=SimplexNoise", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Noise Factor", "NF", "Noise factor", GH_ParamAccess.item, 0.5);
            pManager.AddIntegerParameter("Life Span", "LS", "Maximum age of a star", GH_ParamAccess.item,20);
            pManager.AddIntegerParameter("Max Allowed", "MA", "Maximum number of Stars in scene if value=0 there is no limit for number of stars", GH_ParamAccess.item,50);
            pManager.AddNumberParameter("Emitting Speed", "ES", "Speed of creating stars,Value between 0 and 1, 1=maxSpedd", GH_ParamAccess.item , 1);
            pManager.AddPointParameter("Emitting source", "S", "Emitter source", GH_ParamAccess.list);
            pManager[6].Optional = true;
            pManager.AddPointParameter("Star Attractor", "SA", "Set attractor points which affect stars"  , GH_ParamAccess.list);
            pManager[7].Optional = true;
            pManager.AddNumberParameter("Attraction Factor", "AF" , "Attration Factor" , GH_ParamAccess.item,0);
            pManager.AddBooleanParameter("Reset", "R", "True = reset simulation",GH_ParamAccess.item);

        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stars", "SO", "Output Stars", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Elapsed time", "t", "Elaspsed time", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            tt += 1;
            Brep ibrep = new Brep();
            int Noiseindex = 0;
            int Lifespan=0;
            int MaxAll=0;
            List<Point3d> Es = new List<Point3d>();
            List<Point3d> Sa = new List<Point3d>();
            double factor = 0;
            bool reset = false;
            double Noisefactor = 0;
            double EmitSpeed = 0;
            DA.GetData(0, ref ibrep);
            DA.GetData(1, ref Noiseindex);
            DA.GetData(2, ref Noisefactor);
            DA.GetData(3, ref Lifespan);
            DA.GetData(4, ref MaxAll);
            DA.GetData(5, ref EmitSpeed);
            DA.GetDataList(6, Es);
            DA.GetDataList(7, Sa);
            DA.GetData(8, ref factor);
            DA.GetData(9, ref reset);
            //env.isbrepconnect = (DA.GetData(2, ref ibrep) ? true : false);
            bool isbrepconnect = (DA.GetData(0, ref ibrep) ? true : false);
            bool isEsconnect = (DA.GetDataList(6, Es) ? true : false);
            bool isSaconnect = (DA.GetDataList(7, Sa) ? true : false);
            if (reset)
            {
                part.icle = new List<ParticleAtr>();
            }
            int esi = 0;
            double EmitSpeeed = Math.Abs(EmitSpeed - 1);
            if (part.icle.Count < MaxAll && MaxAll != 0)
            {

                int lpc = part.icle.Count;
                int ma = MaxAll;



                esi = (int)Math.Round((ma - lpc) - ((ma - lpc) * EmitSpeeed));
            }
            if (MaxAll == 0) { esi = 5; }
            
            Random rnd = new Random();
            for (int i=0;i<esi;i++)
            {
                if (isEsconnect)
                {
                    ParticleAtr pt = new ParticleAtr();
                    int Ind = rnd.Next(Es.Count);
                    pt.History.Add(Es[Ind]);
                    part.icle.Add(pt);


                }
                else
                {
                    ParticleAtr pt = new ParticleAtr();
                   
                    
                    pt.History.Add(new Point3d(0, 0, 0));
                    part.icle.Add(pt);
                }

            }
            for (int j = 0; j < part.icle.Count; j++)
            {
                if ((part.icle[j].Age < Lifespan) && (isbrepconnect && (ibrep.IsPointInside(part.icle[j].History[part.icle[j].History.Count - 1], 0.001, true)) || (isbrepconnect == false)))
                {
                    Point3d spt = part.icle[j].History[part.icle[j].History.Count - 1];
                    if (Noiseindex == 1)
                    {
                        Random random = new Random();
                        part.icle[j].Vel += new Vector3d((random.NextDouble() * 2) - 1, (random.NextDouble() * 2) - 1, (random.NextDouble() * 2) - 1) * Noisefactor;
                    }
                    if (Noiseindex == 2)
                    {
                        OpenSimplexNoise opt = new OpenSimplexNoise();
                        double nx = opt.Evaluate(spt.Y + spt.Z, spt.Y, spt.Z, tt);
                        double ny = opt.Evaluate(spt.X, spt.X + spt.Z, spt.Z, tt);
                        double nz = opt.Evaluate(spt.X, spt.Y, spt.Y + spt.X, tt);
                        part.icle[j].Vel += new Vector3d(nx, ny, nz) * Noisefactor;
                    }
                    if (isSaconnect)
                    {
                        Point3d pointt = part.icle[j].History[part.icle[j].History.Count - 1];
                        Vector3d vect = new Vector3d(0, 0, 0);
                        for (int pr = 0; pr < Sa.Count; pr++)
                        {
                            double Dist = pointt.DistanceTo(Sa[pr]);
                            Vector3d vectr = Sa[pr] - pointt;
                            vectr.Unitize();
                            vectr /= Dist;
                            vect += vectr;
                        }
                        vect /= Sa.Count;
                        part.icle[j].Vel += vect * factor;
                    }
                    part.icle[j].History.Add(part.icle[j].History[part.icle[j].History.Count - 1] + part.icle[j].Vel);
                    part.icle[j].Age += 1;
                }
                else
                {
                    part.icle.RemoveAt(j);
                }
            }
            DA.SetDataList(0, part.icle);
            DA.SetData(1, tt);

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
            get { return new Guid("d165dce9-edbb-4a1d-972f-522c7bc7ed64"); }
        }
    }
}