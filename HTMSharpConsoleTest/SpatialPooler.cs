using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMSharpConsoleTest
{
    [Serializable]
    class SpatialPooler
    {
        public uint[] InputDimensions;
        public uint[] ColumnsDimensions;
        public uint NumberOfColumns;
        public uint NumberOfInputs;
        private int RandomSeed;
        /// <summary>
        /// 
        /// </summary>
        public uint ColumnPotentialPoolRadius;
        public float ColumnPotentialPoolConnectionsPercent;
        public bool GlobalInhibiton;
        public uint ActiveColumnsPerInhibitionArea;
        public float LocalAreaDensity;
        public uint StimulusThreshold;
        public float SynapsePermanenceInactiveDecrement;
        public float SynapsePermanenceActiveIncrement;
        /// <summary>
        /// Nupic: synPermConnected
        /// </summary>
        public float SynapsePermanenceConnectedThreshold;
        public float SynapsePermanenceBelowStimulusIncrement;
        public float MinPercentOverlapDutyCycle;
        public uint DutyCyclePeriod;
        public float BoostStrength;
        public bool WrapAround;
        public float SynapsePermanenceMin = 0f;
        public float SynapsePermanenceMax = 1.0f;
        public float SynapsePermanenceTrimThreshold;
        public BitArray Overlaps;
        public BitArray BoostedOverlaps;
        public int UpdatePeriod = 50;
        /// <summary>
        /// what is this
        /// </summary>
        public float InitConnectedPercent = 0.5f;
        public uint IterationNumber;
        public uint LearnIterationNumber;
        BitArray[] PotentialPools;
        float[][] Permanences;
        BitArray[] ConnectedSynapses;
        uint[] ConnectedSynapsesCount;
        double[] tieBreaker;
        Random random;

        public SpatialPooler(
            uint[] inputDimensions,
            uint[] columnsDimensions,
            uint columnPotentialPoolRadius = 16,
            float columnPotentialPoolConnectionsPercent = 0.5f,
            bool globalInhibiton = false,
            float localAreaDensity = -1.0f,
            uint activeColumnsPerInhibitionArea = 10,
            uint stimulusThreshold = 0,
            float synapsePermanenceInactiveDecrement = 0.008f,
            float synapsePermanenceActiveIncrement = 0.05f,
            float synapsePermanenceConnectedThreshold = 0.1f,
            float minPercentOverlapDutyCycle = 0.001f,
            uint dutyCyclePeriod = 1000,
            float boostStrength = 0f,
            int randomSeed = -1,
            bool wrapAround = true)

        {
            
            if (inputDimensions == null) inputDimensions = new uint[] { 32, 32 };
            if (columnsDimensions == null) columnsDimensions = new uint[] { 64, 64 };
            // TO DO:
            // Add checking for all the parameters
            if (inputDimensions.Length != columnsDimensions.Length)
                throw new Exception($"inputDimensions size ({inputDimensions.Length}) must match columnDimensions size ({columnsDimensions.Length})");
            if (boostStrength < 0)
                throw new Exception($"boostStrength ({boostStrength}) must be >= 0");
            //

            RandomSeed = randomSeed;
            if (RandomSeed > -1)
                random = new Random();
            else
                random = new Random(RandomSeed);
            InputDimensions = inputDimensions;
            ColumnsDimensions = columnsDimensions;

            NumberOfInputs = ProductArray(InputDimensions);
            NumberOfColumns = ProductArray(ColumnsDimensions);
            ColumnPotentialPoolRadius = Math.Min(columnPotentialPoolRadius, NumberOfInputs);
            ColumnPotentialPoolConnectionsPercent = columnPotentialPoolConnectionsPercent;
            GlobalInhibiton = globalInhibiton;
            ActiveColumnsPerInhibitionArea = activeColumnsPerInhibitionArea;
            LocalAreaDensity = localAreaDensity;
            StimulusThreshold = stimulusThreshold;
            SynapsePermanenceInactiveDecrement = synapsePermanenceInactiveDecrement;
            SynapsePermanenceActiveIncrement = synapsePermanenceActiveIncrement;
            SynapsePermanenceConnectedThreshold = synapsePermanenceConnectedThreshold;
            SynapsePermanenceBelowStimulusIncrement = SynapsePermanenceConnectedThreshold / 10.0f;
            MinPercentOverlapDutyCycle = minPercentOverlapDutyCycle;
            DutyCyclePeriod = dutyCyclePeriod;
            BoostStrength = boostStrength;
            WrapAround = wrapAround;

            SynapsePermanenceTrimThreshold = SynapsePermanenceActiveIncrement / 2.0f;
            if (SynapsePermanenceTrimThreshold >= SynapsePermanenceConnectedThreshold)
                throw new Exception($"SynapsePermanenceTrimThreshold ({SynapsePermanenceTrimThreshold}) (= SynapsePermanenceActiveIncrement / 2.0f ) must be less than SynapsePermanenceConnectedThreshold ({SynapsePermanenceConnectedThreshold})");

            Overlaps = new BitArray((int)NumberOfColumns);
            BoostedOverlaps = new BitArray((int)NumberOfColumns);

            // INIT POTENTIAL POOLS
            PotentialPools = new BitArray[NumberOfColumns];
            for (int i = 0; i < NumberOfColumns; i++)
                PotentialPools[i] = new BitArray((int)NumberOfInputs);

            //INIT PERMANENCES
            Permanences = new float[NumberOfColumns][];
            for (int i = 0; i < NumberOfColumns; i++)
                Permanences[i] = new float[NumberOfInputs];

            // INIT CONNECTED SYNAPSES
            ConnectedSynapses = new BitArray[NumberOfColumns];
            for (int i = 0; i < NumberOfColumns; i++)
                ConnectedSynapses[i] = new BitArray((int)NumberOfInputs);

            ConnectedSynapsesCount = new uint[NumberOfColumns];

            tieBreaker = new double[NumberOfColumns];
            for (int i = 0; i < NumberOfColumns; i++)
            {
                tieBreaker[i] = 0.01*random.NextDouble();
            }
        }

        
        public BitArray MapPotential(uint index)
        {
            BitArray potential = new BitArray((int)NumberOfInputs);

            return potential;
        }
        public uint MapColumn(uint columnIndex)
        {

            return 0;
        }

        public int ProductArray(int[] array)
        {
            int prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x*y);
                    
            return prod;
        }
        public uint ProductArray(uint[] array)
        {
            uint prod = 1;

            if (array != null)
                prod = array.AsParallel().Aggregate((x, y) => x * y);

            return prod;
        }
    }
}
