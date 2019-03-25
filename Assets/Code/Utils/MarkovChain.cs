using System;
using System.Collections;
using System.Collections.Generic;

namespace MyUtils
{
    public class MarkovChain<T> : IEnumerator<T>
    {
        private float[][] transitonMatrix;
        private T[] states;
        private float[] fsProbs;
        private int curStateNum = 0;
        private Random random;
        private static readonly float DELTA = 0.0001f;
        private bool isReseted;
        //private bool isFinite;

        private static bool TestTransMat(float[][] tmat)
        {
            for(int i = 0; i < tmat.GetLength(0); i++)
            {
                float sum = 0;
                for (int j = 0; j < tmat[0].Length; j++)
                {
                    if (tmat[i][j] < 0 || tmat[i][j] > 1) return false;
                    sum += tmat[i][j];
                }
                if (Math.Abs(1 - sum) > DELTA) return false;
            }
            return true;
        }
        
        private int NextStateNum(float[] nsProbs)
        {
            float r = (float)random.NextDouble();
            for(int i = 0; i < nsProbs.Length; i++)
            {
                r -= nsProbs[i];
                if (r < 0) return i;
            }
            return nsProbs.Length - 1;
        }
        private int NextStateNum()
        {
            return NextStateNum(transitonMatrix[curStateNum]);
        }
        MarkovChain(T[] states, float[][] transMatrix)
        {
            if (states.Length != transMatrix.Length || states.Length != transMatrix[0].Length) throw new ArgumentException("Different sizes");
            if (!TestTransMat(transMatrix)) throw new ArgumentException("Bad TransMatrix");
            transitonMatrix = transMatrix;
            this.states = states;
            random = new Random();
            isReseted = true;

        }
        public MarkovChain(T[] states, float[][] transMat, int firstState) : this(states,transMat)
        {
            fsProbs = new float[states.Length];
            Array.Clear(fsProbs, 0, fsProbs.Length);
            fsProbs[firstState] = 1f;
            curStateNum = firstState;
        }
        public MarkovChain(T[] states, float[][] transMat, float[] firstStateProbabilites) : this(states, transMat)
        {
            float sum = 0;
            foreach (float f in firstStateProbabilites)
                sum += f;
            if (states.Length != firstStateProbabilites.Length || Math.Abs(1 - sum) > DELTA) throw new ArgumentException("Bad fsProbabilites");
            fsProbs = firstStateProbabilites;
            curStateNum = NextStateNum(fsProbs);
        }

        public T Current => states[curStateNum];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (isReseted)
                isReseted = false;
            else
                curStateNum = NextStateNum();
            return true;
        }

        public void Reset()
        {
            curStateNum = NextStateNum(fsProbs);
            isReseted = true;
        }

        public override string ToString()
        {
            return "hui tam";
        }
    }
}
