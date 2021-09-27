using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var system = new LinearEquationSystem(matrix, freeMembers);
            return system.Solve();
        }
    }

    public class LinearEquationSystem
    {
        private const double Epsilon = 1e-6;
        private readonly double[][] matrix;
        private readonly double[] freeMembers;
        public int Height => matrix.Length;
        public int Width => Height > 0 ? matrix[0].Length : 0;
        private int preparedColumnsCount;
        private readonly bool[][] dependentVars;
        private int dependentVarsCount;

        public LinearEquationSystem(double[][] matrix, double[] freeMembers)
        {
            this.matrix = matrix;
            this.freeMembers = freeMembers;
            
            dependentVars = new bool[Height][];
            for (var row = 0; row < Height; row++)
                dependentVars[row] = new bool [Width];
        }

        public void AddMultipliedLine(int resIndex, int addIndex, double multiplier)
        {
            for (var i = 0; i < Width; i++)
            {
                matrix[resIndex][i] += multiplier * matrix[addIndex][i];
                if (Math.Abs(matrix[resIndex][i]) < Epsilon) matrix[resIndex][i] = 0;
            }
            freeMembers[resIndex] += multiplier * freeMembers[addIndex];
            if (Math.Abs(freeMembers[resIndex]) < Epsilon) freeMembers[resIndex] = 0;
        }

        public void MultiplyLine(int resIndex, double multiplier)
        {
            matrix[resIndex] = matrix[resIndex].Select(x => x * multiplier).ToArray();
            freeMembers[resIndex] *= multiplier;
        }

        public void SwitchLines(int i, int j)
        {
            if (i < 0 || i >= Height || j < 0 || j >= Height) return;
            
            var tmpLine = matrix[i];
            matrix[i] = matrix[j];
            matrix[j] = tmpLine;

            var tmp = freeMembers[i];
            freeMembers[i] = freeMembers[j];
            freeMembers[j] = tmp;
            
            var tmpLine2 = dependentVars[i];
            dependentVars[i] = dependentVars[j];
            dependentVars[j] = tmpLine2;
        }

        private bool RowOnlyContainsZeros(int row) => matrix[row].All(x => x == 0);

        private void PrepareColumn(int rowIndex, int columnIndex)
        {
            /*
             * m[r][c] == 1, m[r][!= c] == 0
             * Êàê òîëüêî ìû ïîäãîòîâèëè ñòîëáåö, ñòðîêó ñ åäèíèöåé äâèãàåì íàâåðõ.
             * Òàêèì îáðàçîì, íà êàæäîé èòåðàöèè ïðè çàâåðøåíèè ïîäãîòîâêè ñòîëáöà ìû
             * ìû íà÷èíàåì ðàññìàòðèâàòü ïîòåíöèàëüíûå äåëèòåëè ñ íå ìåíåå ÷åì PreparedColumnsCount-é ñòðîêè.
             *
             * Åñëè ðàññìàòðèâàåìûé äåëèòåëü íå ïîäõîäèò, ðàññìàòðèâàåì ñëåäóþùèé ïî ñòîëáöó
             */

            // Ìàòðèöà ãîòîâà.
            if (preparedColumnsCount == Width) return;

            // Ñëó÷àé: íè îäèí äåëèòåëü â ñòîëáöå íå ïîäîø¸ë (íóëåâîé ñòîëáèê)
            // Çàêàí÷èâàåì îáðàáîòêó. Ñòîëáèê ãîòîâ
            if (rowIndex >= Height)
            {
                preparedColumnsCount++;
                return;
            }

            // ×òîáû m[r][c] == 1, áóäåì äåëèòü âñþ ñòðîêó íà m[r][c] 
            var divider = matrix[rowIndex][columnIndex];
            if (divider == 0)
            {
                // Íà íîëü äåëèòü íåëüçÿ, èù¸ì äðóãîãî êàíäèäàòà äëÿ äåëèòåëÿ
                // (ñëåäóþùåå ÷èñëî â ñòîëáöå)
                PrepareColumn(rowIndex + 1, columnIndex);
                return;
            }

            // Åñëè äåëèòåëü ïîäõîäÿùèé, òî äåëèì.
            // m[r][*] / m[r][c], ÷òîáû m[r][c] == 1
            MultiplyLine(rowIndex, 1 / divider);

            // è âû÷èòàåì å¸ â íóæíûõ êîë-âàõ èç îñòàëüíûõ.
            // m[..][c] - m[r] * m[..][c]
            for (var row = 0; row < Height; row++)
            {
                if (row == rowIndex) continue;
                AddMultipliedLine(row, rowIndex, -matrix[row][columnIndex]);
            }

            // Òåïåðü m[*][c] == 0; m[r][c] == 1.
            // Äâèãàåì ñòðî÷êó íàâåðõ.
            
            dependentVars[rowIndex][columnIndex] = true;
            if (rowIndex != preparedColumnsCount)
                SwitchLines(rowIndex, preparedColumnsCount);
            preparedColumnsCount++;
            dependentVarsCount++;

        }

        public double[] Solve()
        {
            // Ïðèâîäèì ê ñòóïåí÷àòîìó âèäó
            while (preparedColumnsCount < Width)
                PrepareColumn(dependentVarsCount, preparedColumnsCount);

            // Òåïåðü ìàòðèöà èìååò ñòóïåí÷àòûé âèä! Ìîæíî ýòî âûíåñòè â îòäåëüíûé ìåòîä, êñòàòè.

            // Ïðîâ¸äåì èññëåäîâàíèå òîãî, ñîâìåñòíà ëè îíà.
            // Åñëè íåò -- ñðàçó âîçâðàùàåì ïóñòîå ìíîæåñòâî ðåøåíèé.
            for (int row = 0; row < Height; row++)
            {
                if (RowOnlyContainsZeros(row) && freeMembers[row] != 0)
                    throw new NoSolutionException("No solution!");
            }
            // Òåïåðü ðàññìàòðèâàåì ñîâìåñòíóþ ìàòðèöó.
            
            var solution = new double[Width];
            var definedVars = new bool[Width];
            
            for (var row = Height - 1; row >= 0; row--)
            {
                if (RowOnlyContainsZeros(row)) continue;
                var encounteredDependentVar = false;
                for (var column = Width - 1; column >= 0 && !encounteredDependentVar; column--)
                {
                    // x_column -- çàâèñèìàÿ ïåðåìåííàÿ
                    // !!! TODO
                    // if (row == column && _dependentVars[column])
                    if (dependentVars[row][column])
                    {
                        // ïîäñòàâëÿåì âñå íàéäåííûå ðàíåå çíà÷åíèÿ íåçàâèñèìûõ ïåðåìåííûõ
                        var sum = 0.0;
                        for (var i = column + 1; i < Width; i++)
                            sum += matrix[row][i] * solution[i];
                        var tmp = (freeMembers[row] - sum) / matrix[row][column];
                        solution[column] = tmp;
                        
                        // ïîäñ÷¸ò â ñòðîêå îêàí÷èâàåòñÿ, êîãäà ìû äîõîäèì äî çàâèñèìîé ïåðåìåííîé
                        encounteredDependentVar = true;
                    }
                    else // x_column -- ñâîáîäíàÿ ïåðåìåííàÿ
                    {
                        // Åñëè ïåðåìåííàÿ îïðåäåëåíà â ðåøåíèè, òî íè÷åãî íå äåëàåì.
                        // Åñëè æå íå îïðåäåëåíà, òî ïîäñòàâëÿåì åé ëþáîå çíà÷åíèå
                        if (!definedVars[column]) 
                            solution[column] = 0;
                    }
                    
                    definedVars[column] = true;
                }
            }

            return solution;
        }
    }
}
© 2021 GitHub, I