using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ski_Resort
{
    static class Extensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
    class Process
    {
        public string Procesar(string Filename, char Separador)
        {
            string msjReturn = "";
            // Lista de los path Posibles
            List<List<string>> Paths = new List<List<string>>();
            List<string> Path = new List<string>();
            List<string> PathInicial = new List<string>();
            //Leemos el archivo
            string[] lines = File.ReadAllLines(Filename);
            int filas = Convert.ToInt32(lines[0].Split(Separador)[0]);
            int columnas = Convert.ToInt32(lines[0].Split(Separador)[0]);
            int[,] matriz = new int[filas, columnas];

            //Lo convertimos en una matriz para empezar a trabajar
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fila = lines[i].Split(Separador);
                for (int j = 0; j < fila.Length; j++)
                {
                    //ponemos i-1 ya que empezamos a leer desde la segunda linea del archivo
                    matriz[(i-1), j] = Convert.ToInt32(fila[j]);
                }
            }
            //Empezamos a recorrer la matriz
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    //Iniciamos el camino inicial agregandole el numero desde donde estamos partiendo
                    Path = new List<string>();
                    PathInicial = new List<string>();
                    PathInicial.Add(matriz[i, j] + "");
                    Path.Add(matriz[i, j]+ "");
                    ValidarAdyacentes(i, j, ref Path, matriz, ref Paths, PathInicial);

                }
            }

            List<List<string>> FinalPaths = new List<List<string>>();
            List<string> pathf = new List<string>();
            
            int CalcDropPath = 0;

            //Usamos LINQ para obtener el camino mas grande
            var LengthCalculated1 = Paths.Max(x => x.Count);

            FinalPaths = Paths.Where(x => x.Count == LengthCalculated1).ToList();

            //Si hay mas de 1 ruta con el tamaño mayor iguales entonces calculamos la caida maxima de cada uno
            if (FinalPaths.Count > 1)
            {
                var PathsF = new List<List<string>>();
                
                foreach (List<string> caminos in FinalPaths)
                {
                   var DropPath = Int32.Parse(caminos.ElementAt(0)) - Int32.Parse(caminos.ElementAt(caminos.Count - 1));
                    if (DropPath > CalcDropPath)
                    {
                        CalcDropPath = DropPath;
                    }
                }
                //Habiendo calculado la mayor caida, obtenemos los paths que tengan esa mayor caida
                foreach (List<string> caminos in FinalPaths)
                {
                    var DropPath = Int32.Parse(caminos.ElementAt(0)) - Int32.Parse(caminos.ElementAt(caminos.Count - 1));
                    if (DropPath == CalcDropPath)
                    {
                        PathsF.Add(caminos);
                    }
                }
                if (PathsF.Count > 1)
                {
                    msjReturn = "There is more than one calculated final path with the same length and with the same drop:\nLength of Calculated Path: " + LengthCalculated1 + "\nDrop of Calculated Path: " + CalcDropPath + "\nPaths: (";
                    foreach (var camino in PathsF)
                    {
                        foreach (var posicion in camino)
                        {
                            msjReturn = msjReturn + posicion + " ";
                        }
                        msjReturn = msjReturn.Remove(msjReturn.Length - 1);
                        msjReturn = msjReturn + ")\n";
                    }
                }
                else
                {
                    msjReturn = "Length of Calculated Path: " + LengthCalculated1 + "\nDrop of Calculated Path: " + CalcDropPath + "\nPath: (";
                    foreach (var camino in PathsF)
                    {
                        foreach (var posicion in camino)
                        {
                            msjReturn = msjReturn + posicion + " ";
                        }
                        msjReturn = msjReturn.Remove(msjReturn.Length - 1);
                        msjReturn = msjReturn + ")\n";
                    }
                }

            }
            else
            {
                foreach (List<string> caminos in FinalPaths)
                {
                    var DropPath = Int32.Parse(caminos.ElementAt(0)) - Int32.Parse(caminos.ElementAt(caminos.Count - 1));
                    msjReturn = "Length of Calculated Path: " + LengthCalculated1 + "\nDrop of Calculated Path: " + CalcDropPath + "\nPath: (";
                    foreach (var camino in FinalPaths)
                    {
                        foreach (var posicion in camino)
                        {
                            msjReturn = msjReturn + posicion + " ";
                        }
                        msjReturn = msjReturn.Remove(msjReturn.Length - 1);
                        msjReturn = msjReturn + ")\n";
                    }
                }
            }
            return msjReturn;
        }
        private void ValidarAdyacentes(int i, int j, ref List<string> Path, int [,] matriz, ref List<List<string>> Paths, [Optional] List<string> PathInicial )
        {
            
            //Validamos caminos, si es la primera linea no podemos mirar el norte
            if (i == 0)
            {
                //Si es la primera columna solo podemos ir al este o al sur
                if (j == 0)
                {
                    //leemos los path al este validando que no sea la ultima columna
                    if ((j + 1) < matriz.GetLength(1))
                    {
                        if (matriz[i, j] > matriz[i, (j + 1)])
                        {
                            //Agregamos el paso siguiente a nuestra lista de caminos
                            Path.Add(matriz[i, (j + 1)] + "");
                            ValidarAdyacentes(i, (j + 1), ref Path, matriz, ref Paths);
                            //Lista para agregar a la lista de caminos
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            //Limpiamos el camino para seguir evaluando las demas posiciones
                            Path.RemoveAt(Path.Count - 1);
                            
                        }
                    }
                    //leemos path al sur validando que no sea la ultima fila
                    if ((i + 1) < matriz.GetLength(0))
                    {
                        if (matriz[i, j] > matriz[(i + 1), j])
                        {
                            Path.Add(matriz[(i + 1), j] + "");
                            ValidarAdyacentes((i + 1), j, ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);

                        }
                    }
                }
                //Validamos si podemos ir al oeste, al este y al sur
                else
                {
                    //Al oeste
                    if ((j - 1) >= 0)
                    {
                        if (matriz[i, j] > matriz[i, (j - 1)])
                        {
                            Path.Add(matriz[i, (j - 1)] + "");
                            ValidarAdyacentes(i, (j - 1), ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                    //Al este
                    if ((j + 1) < matriz.GetLength(1))
                    {
                        if (matriz[i, j] > matriz[i, (j + 1)])
                        {
                            Path.Add(matriz[i, (j + 1)] + "");
                            ValidarAdyacentes(i, (j + 1), ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                    //Al sur
                    if ((i + 1) < matriz.GetLength(0))
                    {
                        if (matriz[i, j] > matriz[(i + 1), j])
                        {
                            Path.Add(matriz[(i + 1), j] + "");
                            ValidarAdyacentes((i + 1), j, ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                }
            }
            else
            {
                if (j == 0)
                {
                    //Validamos al norte
                    if ((i - 1) >= 0)
                    {

                        if (matriz[i, j] > matriz[(i - 1), j])
                        {
                            Path.Add(matriz[(i - 1), j] + "");
                            ValidarAdyacentes((i - 1), j, ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }

                    }
                    //leemos los path al este validando que no sea la ultima columna
                    if ((j + 1) < matriz.GetLength(1))
                    {
                        if (matriz[i, j] > matriz[i, (j + 1)])
                        {
                            Path.Add(matriz[i, (j + 1)] + "");
                            ValidarAdyacentes(i, (j + 1), ref Path, matriz,ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                    //leemos path al sur validando que no sea la ultima fila
                    if ((i + 1) < matriz.GetLength(0))
                    {
                        if (matriz[i, j] > matriz[(i + 1), j])
                        {
                            Path.Add(matriz[(i + 1), j] + "");
                            ValidarAdyacentes((i + 1), j, ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                }
                //Validamos si podemos ir al oeste, al este, al norte y al sur
                else
                {
                    //Al oeste
                    if ((j - 1) >= 0)
                    {
                        if (matriz[i, j] > matriz[i, (j - 1)])
                        {
                            Path.Add(matriz[i, (j - 1)] + "");
                            ValidarAdyacentes(i, (j - 1), ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                    //Al este
                    if ((j + 1) < matriz.GetLength(1))
                    {
                        if (matriz[i, j] > matriz[i, (j + 1)])
                        {
                            Path.Add(matriz[i, (j + 1)] + "");
                            ValidarAdyacentes(i, (j + 1), ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                    //al norte
                    if ((i - 1) >= 0)
                    {

                        if (matriz[i, j] > matriz[(i - 1), j])
                        {
                            Path.Add(matriz[(i - 1), j] + "");
                            ValidarAdyacentes((i - 1), j, ref Path, matriz, ref Paths);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }

                    }
                    //Al sur
                    if ((i + 1) < matriz.GetLength(0))
                    {
                        if (matriz[i, j] > matriz[(i + 1), j])
                        {
                            Path.Add(matriz[(i + 1), j] + "");
                            ValidarAdyacentes((i + 1), j, ref Path, matriz, ref Paths, Path);
                            List<string> AddList = Path.Clone();
                            Paths.Add(AddList);
                            Path.RemoveAt(Path.Count - 1);
                        }
                    }
                }
            }
        }
    }
}
