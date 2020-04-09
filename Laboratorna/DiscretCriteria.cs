using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorna
{
    class DiscretCriteria
    {
        private List<double> x;
        private List<int> frequency;
        private List<int> groupFrequency;
        private List<double> group;
        private List<double> resHi;
        private double h;
        private int n;
        private int r;
        private int count;
        private double a;
        private double b;
        private List<double> pI;
        private List<double> pIPyson;

        public DiscretCriteria()
        {
            this.x = new List<double>();
            this.frequency = new List<int>();
            this.groupFrequency = new List<int>();
            this.group = new List<double>();
            this.pI = new List<double>();
            this.pIPyson = new List<double>();
            this.resHi = new List<double>();
            this.h = 0;
            this.n = 0;
            this.r = 0;
            this.count = 0;
            this.a = 0;
            this.b = 0;
        }

        public void ReadFromFile(String fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            String s = null;
            n = 0;
            while ((s = sr.ReadLine()) != null)
            {
                double valueX = double.Parse(s.Split()[0]);
                x.Add(valueX);
                int valueFrequency = int.Parse(s.Split()[1]);
                this.frequency.Add(valueFrequency);
                n += valueFrequency;
            }

            sr.Close();
        }

        public void ToTable(DataGridView dw)
        {
            dw.ColumnCount = x.Count;
            dw.RowCount = 2;
            for (int i = 0; i < x.Count; i++)
            {
                dw[i, 0].Value = x[i].ToString();
                dw[i, 1].Value = frequency[i].ToString();
            }
        }

        public void CreateGroup()
        {

            r =(int)Math.Truncate(Math.Log(n, 2));
            for (int i = 0; i < x.Count; i++)
            {
                for (int j = 0; j < x.Count - i - 1; j++)
                {
                    if (x[j] > x[j + 1])
                    {
                        double buf = x[j];
                        x[j] = x[j + 1];
                        x[j + 1] = buf;
                        int bufNew = frequency[j];
                        frequency[j] = frequency[j + 1];
                        frequency[j + 1] = bufNew;
                    }
                }
            }

            a = x[0];
            b = x[x.Count - 1];
            h = (b - a) / r;
            int controlN = 0;
            for (int i = 0; i < r + 1; i++)
            {
                group.Add(a + i * h);
                groupFrequency.Add(0);
                for (int j = 0; j < x.Count; j++)
                {
                    if (x[j] >= group[i] && x[j] < (group[i] + h))
                    {
                        groupFrequency[i] += frequency[j];
                    }

                }

                controlN += groupFrequency[i];
            }
        }

        public void ToTableGroup(DataGridView dw)
        {
            dw.ColumnCount = r + 1;
            dw.RowCount = 7;
            for (int i = 0; i < r + 1; i++)
            {
                dw[i, 0].Value = "[" + group[i].ToString() + "," + (group[i] + h).ToString() + ")";
                dw[i, 1].Value = groupFrequency[i].ToString();
            }
        }

        public double MatSpod()
        {
            double res = 0;
            for (int i = 0; i < x.Count; i++)
            {
                res += x[i] * frequency[i] / n;
            }

            return res;
        }

        public double Dispercia()
        {
            double res = 0;
            double ms = MatSpod();
            for (int i = 0; i < x.Count; i++)
            {
                res += Math.Pow(x[i] - ms, 2) * frequency[i] / n;
            }

            return res;
        }

        public double GustinaNormal(double x)
        {
            double ms = MatSpod();
            double d = Dispercia();
            double res = Math.Exp((-Math.Pow(x - ms, 2)) / 2.0 / d) / Math.Sqrt(2 * Math.PI * d);
            return res;
        }

        public double Function(double alfa, double beta)
        {
            double s = 0;
            double hI = (beta - alfa) / 200;
            for (int i = 0; i <= 200; i++)
            {
                s += GustinaNormal(alfa + hI * i) * hI;
            }

            return s;
        }

        public double Pyason(int x, double lamda)
        {
            int f = 1;
            for (int i = 1; i <= x; i++)
            {
                f *= i;
            }

            return Math.Pow(lamda, x) * Math.Exp(-lamda) / f;
        }

        public double HiDvaPyason(double lamda)
        {
            double res = 0;
            double s = 0;
            pIPyson.Clear();
            resHi.Clear();
            for (int i = 0; i < r + 1; i++)
            {
                double p = Pyason((int)group[i],lamda);
                pIPyson.Add(p);
                s += p;
                res +=  Math.Pow(groupFrequency[i] -n*p, 2)/n/p;
                resHi.Add(Math.Pow(groupFrequency[i] - n * p, 2) / n / p);

            }

            MessageBox.Show("s= " + s);
            return res;
        }

        public double HiDvaNormal()
        {
            double res = 0;
            double s = 0;
            pI.Clear();
            for (int i = 0; i < r + 1; i++)
            {
                double p = Function(group[i], group[i] + h);
                pI.Add(p);
                s += p;
                res += Math.Pow(groupFrequency[i] - n * p, 2) / n / p;
            }

            return res;
        }

        public void ToTablePosibility(DataGridView dw)
        {
            
            for (int i = 0; i < r + 1; i++)
            {
                dw[i, 2].Value = Math.Round(pI[i],3).ToString();
                dw[i, 4].Value = Math.Round(n * pI[i], 3).ToString();

            }
        }

        public void ToTablePosibility1(DataGridView dw)
        {

            for (int i = 0; i < r + 1; i++)
            {
                dw[i, 3].Value = Math.Round(pIPyson[i], 3).ToString();
                dw[i, 5].Value = Math.Round(n * pIPyson[i], 3).ToString();
                dw[i, 6].Value = Math.Round(resHi[i], 3).ToString();
            }
        }
       
    }
}
