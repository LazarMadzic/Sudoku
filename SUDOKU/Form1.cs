using System.Diagnostics;

namespace SUDOKU
{
    public partial class SUDOKU : Form
    {
        string current_game { get; set; } = string.Empty;
        string currentPickedNum { get; set; } = string.Empty;
        bool erase = false;
        bool winCondition { get; set; } = false;
        private int numberNonZero = 0;
        SudokuCell prevBtn = null;
        SudokuCell[,] cells = new SudokuCell[9, 9];
        int [,] grid_num = new int[9, 9];
        SudokuCell[ ] padN = new SudokuCell[10];
        char[] chars = { '1','2','3','4', '5', '6', '7', '8', '9', 'X' } ;
        List<int> used= new List<int>(10);
        private Game gm = null;
        private string path = @"..\..\..\States\last.txt";
        private string path_easy = @"..\..\..\States\0.txt";
        private string path_med = @"..\..\..\States\2.txt";
        private string path_hard = @"..\..\..\States\4.txt";
        public SUDOKU()
        {
            List<string> lastGame = new List<string>();
            foreach (string line in System.IO.File.ReadLines(path))
            {
                lastGame.Add(line);

            }

            System.Console.WriteLine(lastGame[1]);


            InitializeComponent();

            createCells();
            intialiseUsed();
            current_game = lastGame[1];
            gm = new Game(panel1,panel2, ref cells, ref numberNonZero, current_game);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            reset_cells();
            this.numberNonZero = 0;
            gm = new Game(panel1, panel2, ref cells, ref numberNonZero, current_game);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void createCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Create 81 cells for with styles and locations based on the index
                    cells[i, j] = new SudokuCell();
                    cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 20);
                    cells[i, j].Size = new Size(40, 40);
                    cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                    cells[i, j].Location = new Point(i * 40, j * 40);
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    cells[i, j].FlatStyle = FlatStyle.Flat;
                    cells[i, j].FlatAppearance.BorderColor = Color.Black;
                    cells[i, j].X = i;
                    cells[i, j].Y = j;

                    // Assign key press event for each cells
                    cells[i, j].KeyPress += cell_keyPressed;
                    cells[i, j].Click += cell_Click;
                    panel2.Controls.Add(cells[i, j]);
                }
            }
            numberPad();
        }

        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            var cell = sender as SudokuCell;

            // Do nothing if the cell is locked
            if (cell.IsLocked)
                return;

            int value;

            // Add the pressed key value in the cell only if it is a number
            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                // Clear the cell value if pressed key is zero
                if (value == 0)
                    cell.Clear();
                else
                    cell.Text = value.ToString();

                cell.ForeColor = SystemColors.ControlDarkDark;
            }
        }

        private void numberPad() {
            int counter = 0;

            for (int i = 0; i < 10; i++)
            {
                
                    padN[ i] = new SudokuCell();
                    padN[ i].Font = new Font(SystemFonts.DefaultFont.FontFamily, 15);
                    padN[ i].Size = new Size(30, 30);
                    padN[i].ForeColor = SystemColors.ControlDarkDark;
                    padN[i].Location = new Point(i * 30);
                    padN[ i].FlatStyle = FlatStyle.Flat;
                    padN[ i].FlatAppearance.BorderColor = Color.Black;
                   // padN[ i].X = j;
                    padN[ i].Y = i;
                    padN[ i].Text = chars[counter].ToString();
                //TODO add on mouse klick event
                if (i != 9)
                {
                    padN[i].Value = i + 1;

                    padN[i].Click += padN_Click;

                }
                    flowLayoutPanel1.Controls.Add(padN[ i]);
                    counter++;
                
            }
            padN[9].Click += cell_erase;


        }

        private void intialiseUsed() {

            for (int i = 0; i < 10; i++)
            {
                used.Add(0);
            }
        
        
        }

        private void cell_Click(object sender, EventArgs e) {
            if (erase) {
                if (!(sender as SudokuCell).IsLocked) {
                    (sender as SudokuCell).Text = string.Empty;
                    (sender as SudokuCell).Value = 0;
                    numberNonZero--;

                }
                return;


            }
            

            if (!string.IsNullOrEmpty(currentPickedNum))
            {
                int x = Int32.Parse(currentPickedNum);
                if (!(sender as SudokuCell).IsLocked)
                {
                    bool result = possibePlace((sender as SudokuCell).X, (sender as SudokuCell).Y, x);

                    if (result)
                    {

                        (sender as SudokuCell).Text = currentPickedNum;
                        (sender as SudokuCell).Value = Int32.Parse(currentPickedNum.ToString());
                        (sender as SudokuCell).ForeColor = Color.Navy;
                        used[x]++;
                        numberNonZero++;
                        System.Console.WriteLine(numberNonZero.ToString());
                    }

                    paintCells();
                    CheckWin();
                }
            }
        
        }

        private void CheckWin() {
            if (numberNonZero == 81) {
                System.Console.WriteLine("POPUP");
                Form f2 = new PopUp();
                f2.Show();
            }
        
        
        }
        

        private bool possibePlace(int i, int j, int target) {
            bool flag = true;
            //System.Console.WriteLine(i+ "  " + j);
            //System.Console.WriteLine("target"+ target.ToString());
            for (int j2 = 0; j2 < 9; j2++) {
                if (cells[i, j2].Value == target) {
                    flag = false;
                    //System.Console.WriteLine(cells[i, j2].Value.ToString());
                    //System.Console.WriteLine("PASS 1/2 ");
                    break;                
                
                } 
            }
            //System.Console.WriteLine("PASS 1 ");
            if (flag) {
                for (int j2 = 0; j2 < 9; j2++)
                {
                    if (cells[ j2,j].Value == target)
                    {
                        flag = false;
                        break;

                    }
                }
                //System.Console.WriteLine("PASS 2 ");

                if (flag) {
                    int x0 = (i / 3) * 3;
                    int y0= (j / 3) * 3;
                    System.Console.WriteLine(x0 + "  " + y0);
                    for (int j2 = 0; j2 < 3; j2++) {
                        for (int j3 = 0; j3 < 3; j3++)
                        {

                            if(cells[x0+j2, y0+j3].Value==target)return false;

                        }
                    }
                
                
                
                }


            }
            return flag;

        }


        private void padN_Click(object sender, EventArgs e)
        {
            erase=false;
            if (prevBtn == null)
            {
                prevBtn = sender as SudokuCell;


            }
            else
            {

                prevBtn.FlatAppearance.BorderColor = Color.Black;
                prevBtn= sender as SudokuCell;

            }

            System.Console.WriteLine((sender as Button).Text);
            currentPickedNum = (sender as Button).Text;
            (sender as Button).FlatAppearance.BorderColor = Color.Gold;

            paintCells();
        }

        private void paintCells() {
            int x = Int32.Parse(currentPickedNum);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    //cells[i, j].FlatAppearance.BorderColor = Color.Black;

                    if (cells[i, j].Value == x)
                    {
                        //cells[i, j].FlatAppearance.BorderColor = Color.Green;


                        cells[i, j].BackColor = Color.Cyan;
                    }
                }
            }



        }

        public void print_matrix() {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    System.Console.Write(cells[j,i].Value + " ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }

        public void solve(object sender, EventArgs e) {
            //print_matrix();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            solve_func();
            stopwatch.Stop();
            System.Console.WriteLine(stopwatch.Elapsed);

        }

        private bool solve_func() {
            //print_matrix();
            Tuple<int, int> result = find_free();
            if (result.Item1 == -1 && result.Item2 == -1)
            {
                return true;
            }

            for (int i = 1; i < 10; i++)
            {
                if (valid_position(i, result.Item1, result.Item2)) {
                    cells[result.Item2, result.Item1].Value = i;
                    cells[result.Item2, result.Item1].Text = i.ToString();
                    this.currentPickedNum = i.ToString();
                    paintCells();
                    if (solve_func()) {
                        return true;
                    
                    }
                    cells[result.Item2, result.Item1].Value = 0;
                    cells[result.Item2, result.Item1].Text = String.Empty;
                }
            }


            return false;
        }

        bool valid_position(int val, int x, int y) {
            //Check row
            for (int i = 0; i < 9; i++)
            {
                if (cells[i,x].Value == val && i != y) {
                    return false;
                
                }
            }
            //Check column
            for (int i = 0; i < 9; i++)
            {
                if (cells[y, i].Value == val && i != x)
                {
                    return false;

                }
            }
            //Check square
            int x_start = (x / 3) * 3;
            int y_start = (y / 3) * 3;

            for (int i = x_start; i < x_start + 3; i++)
            {
                for (int j = y_start; j < y_start + 3; j++)
                {
                    //System.Console.Write(cells[j, i].Value + " ");
                    if (cells[j, i].Value == val && i != x && j != y)
                    {
                        return false;

                    }
                }
                   // System.Console.WriteLine();
                }
               // System.Console.WriteLine();

            return true;
        }

        Tuple<int, int> find_free() {

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[j,i].Value == 0) {

                       return new Tuple<int, int>(i, j);
                    }
                }
            }

            return new Tuple<int, int>(-1,-1);
        }

        private void cell_erase(object sender, EventArgs e)
        {
            
            
            if (erase)
            {
                prevBtn.FlatAppearance.BorderColor = Color.Black;
                erase = false;
                prevBtn = null;
            }
            else {

                prevBtn.FlatAppearance.BorderColor = Color.Black;
                erase = true;
                prevBtn = sender as SudokuCell;
                prevBtn.FlatAppearance.BorderColor = Color.Red;


            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void new_game(object sender, PaintEventArgs e)
        {

        }

        private void SUDOKU_Load(object sender, EventArgs e)
        {
            Random r = new Random();
            //int rInt = r.Next(0, 100);
            var checkedButton = panel1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            int line_number;
            reset_cells();
            switch (checkedButton.Text)
            {
                case "Easy":
                    line_number = r.Next(1, 10001);
                    this.current_game = read_string_from_file(line_number, path_easy);
                    gm = new Game(panel1, panel2, ref cells, ref numberNonZero, this.current_game);
                    break;
                case "Medium":
                    line_number = r.Next(1, 10001);
                    this.current_game = read_string_from_file(line_number, path_med);
                    gm = new Game(panel1, panel2, ref cells, ref numberNonZero, this.current_game);
                    break;
                case "Hard":
                    line_number = r.Next(1, 10000);
                    this.current_game = read_string_from_file(line_number, path_hard);
                    gm = new Game(panel1, panel2, ref cells, ref numberNonZero, this.current_game);
                    break;
                default:
                    break;
            }
            
        }

        private void reset_cells() {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                    cells[i,j].Value = 0;
                    cells[i, j].Text = String.Empty;
                    cells[i, j].IsLocked = false;
                    currentPickedNum = String.Empty;
                }
            }
            this.numberNonZero = 0; 
           // paintCells();
        }

        private string read_string_from_file(int line_number,string new_path) {
            int counter = 1;
            
            foreach (string line in System.IO.File.ReadLines(new_path))
            {
               // System.Console.WriteLine(counter);
                if (counter == line_number) {
                    System.Console.WriteLine(line);
                    return line;

                }
                counter++;  
            }

            return "";
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}