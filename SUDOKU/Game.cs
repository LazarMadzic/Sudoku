using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUDOKU
{
    
    internal class Game
    {
        private string board = "";
        private Panel p1 { get; set; }
        private Panel p2 { get; set; }

        private SudokuCell[,] matrix; 

        

        public Game(Panel ps1, Panel ps2, ref SudokuCell[,] mat, ref int currentNonZero,  string game_setup) { 
            this.p1 = ps1;
            this.p2 = ps2;
            this.matrix = mat;

            //var checkedButton = p1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            //string difficulty

            


           
            
            int counter = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (game_setup[counter] != '0')
                    {
                        mat[j, i].Text = game_setup[counter].ToString();
                        //System.Console.WriteLine(mat[i, j].Text);
                        mat[j, i].Value = game_setup[counter] - '0';
                        mat[j, i].IsLocked = true;
                        currentNonZero++;
                    }
                    else {
                        mat[j, i].Value = game_setup[counter] - '0';



                    }


                        
                    counter++;

                }
            }
            
            

        }


        private void PopulateBoard(int x, string combination) { 
        
        }








    }
}
