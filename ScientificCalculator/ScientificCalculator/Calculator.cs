using Calculator.MathLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace ScientificCalculator
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();

        }
        private void FillBucket()
        {
            foreach (var operation in _symbolMap)
            {
                Button button = new Button();
                button.Text = operation.Value.Symbol;
                button.Dock = DockStyle.Fill;
                button.Click += new System.EventHandler(this.Button_clicked);
                _buttonList.Add(button);
                _inputPanelOperators.Controls.Add(button);
            }
            Button button1 = new Button();
            button1.Text = ".";
            button1.Dock = DockStyle.Fill;
            button1.Click += new System.EventHandler(this.Button_clicked);
            _buttonList.Add(button1);
            _inputPanelOperators.Controls.Add(button1);
        }

        private void FillDefaultButtons()
        {
            Button clear = new Button()
            {
                Text = "CE",
                Dock = DockStyle.Fill,
                BackColor = Color.BlanchedAlmond,

            };
            clear.Click += new EventHandler(this.ClearAllClicked);
            Button backspace = new Button()
            {
                Text = "Backspace",
                Dock = DockStyle.Fill,
                BackColor = Color.BlanchedAlmond,

            };
            backspace.Click += new EventHandler(this.BackspaceClicked);
            Button equal = new Button()
            {
                Text = "=",
                Dock = DockStyle.Fill,

            };
            equal.Click += new EventHandler(this.EqualToClicked);
            Button openBracket = new Button()
            {
                Text = "(",
                Dock = DockStyle.Fill,

            };
            Button closeBracket = new Button()
            {
                Text = ")",
                Dock = DockStyle.Fill,

            };
            Button zero = new Button()
            {
                Text = "0",
                Dock = DockStyle.Fill,
                BackColor = Color.White,

            };
            zero.Click += new System.EventHandler(this.Button_clicked);
            openBracket.Click += new System.EventHandler(this.Button_clicked);
            closeBracket.Click += new System.EventHandler(this.Button_clicked);
            _buttonList.Add(zero);
            _buttonList.Add(openBracket);
            _buttonList.Add(closeBracket);
            this._inputPanelDefaults.Controls.Add(equal, 0, 0);
            this._inputPanelDefaults.Controls.Add(clear, 1, 0);
            this._inputPanelDefaults.Controls.Add(backspace, 2, 0);
            this._inputPanelDefaults.Controls.Add(openBracket, 0, 4);
            this._inputPanelDefaults.Controls.Add(zero, 1, 4);
            this._inputPanelDefaults.Controls.Add(closeBracket, 2, 4);


            int num = 1;
            for (int i = 1; i < _inputPanelDefaults.RowCount - 1; i++)
            {
                for (int j = 0; j < _inputPanelDefaults.ColumnCount; j++)
                {
                    Button button = new Button()
                    {
                        Text = num.ToString(),
                        Dock = DockStyle.Fill,
                        BackColor = Color.White,
                    };
                    button.Click += new System.EventHandler(this.Button_clicked);
                    _inputPanelDefaults.Controls.Add(button, j, i);
                    _buttonList.Add(button);
                    num++;
                }
            }


        }

        private Stack<double> _storage;
        private Dictionary<string, SymbolInformation> _symbolMap;
        private string _input;
        private ExpressionEvaluator eval;
        private System.Windows.Forms.SplitContainer _calculatorLayout;
        private System.Windows.Forms.SplitContainer _panelLayout;
        private System.Windows.Forms.TextBox _inputTextBox;
        private System.Windows.Forms.TableLayoutPanel _inputPanelOperators;
        private System.Windows.Forms.TableLayoutPanel _inputPanelDefaults;
        private System.Windows.Forms.Label _outputExpression;
        private List<System.Windows.Forms.Button> _buttonList;
        private Button _memoryRead;
        private Button _memoryWrite;
        private Button _memoryRemove;
        //private Button _editButton;
        private Button _exitButton;
        private Button _helpButton;
        private double answer;

        private void InitializeComponent()
        {
            this.eval = new ExpressionEvaluator();
            this._symbolMap = new Dictionary<string, SymbolInformation>();
            this._calculatorLayout = new System.Windows.Forms.SplitContainer();
            this._panelLayout = new System.Windows.Forms.SplitContainer();
            this._inputTextBox = new System.Windows.Forms.TextBox();
            this._inputPanelOperators = new System.Windows.Forms.TableLayoutPanel();
            this._inputPanelDefaults = new System.Windows.Forms.TableLayoutPanel();
            this._outputExpression = new System.Windows.Forms.Label();
            this._storage = new Stack<double>();
            this._memoryRead = new Button();
            this._memoryWrite = new Button();
            this._memoryRemove = new Button();
            //this._editButton = new Button();
            this._exitButton = new Button();
            this._helpButton = new Button();
            this._buttonList = new List<Button>();

            //bringing the symbols of operators from Json file to map them with buttons 
            _symbolMap = JsonConvert.DeserializeObject<Dictionary<string, SymbolInformation>>(File.ReadAllText("Config\\OperationsConfiguration.json"));

            //initialising the input expression string. This will be sent for evaluation to the class library method
            _input = string.Empty;

            ((System.ComponentModel.ISupportInitialize)(this._calculatorLayout)).BeginInit();
            this._calculatorLayout.Panel1.SuspendLayout();
            this._calculatorLayout.Panel2.SuspendLayout();
            this._calculatorLayout.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)(this._panelLayout)).BeginInit();
            this._panelLayout.Panel1.SuspendLayout();
            this._panelLayout.Panel2.SuspendLayout();
            this._panelLayout.SuspendLayout();

            this.SuspendLayout();

            // 
            // _calculatorLayout : It is a horizontal splitContainer dividing screen into display panel and button panel
            // 
            this._calculatorLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._calculatorLayout.IsSplitterFixed = true;
            this._calculatorLayout.Location = new System.Drawing.Point(0, 0);
            this._calculatorLayout.Name = "_calculatorLayout";
            this._calculatorLayout.Orientation = System.Windows.Forms.Orientation.Horizontal;

            // 
            // _calculatorLayout.Panel1
            // 
            this._calculatorLayout.Panel1.BackColor = System.Drawing.Color.AntiqueWhite;
            this._calculatorLayout.Panel1.Controls.Add(this._inputTextBox);
            this._calculatorLayout.Panel1.Controls.Add(this._outputExpression);
            this._calculatorLayout.Panel1.Controls.Add(this._memoryRead);
            this._calculatorLayout.Panel1.Controls.Add(this._memoryWrite);
            this._calculatorLayout.Panel1.Controls.Add(this._memoryRemove);
            //this._calculatorLayout.Panel1.Controls.Add(this._editButton);
            this._calculatorLayout.Panel1.Controls.Add(this._helpButton);
            this._calculatorLayout.Panel1.Controls.Add(this._exitButton);

            // 
            // _calculatorLayout.Panel2
            // 
            this._calculatorLayout.Panel2.Controls.Add(this._panelLayout);
            this._calculatorLayout.Size = new System.Drawing.Size(600, 500);
            this._calculatorLayout.SplitterDistance = 90;

            //
            //_memoryRead Button
            //
            this._memoryRead.Text = "MR";
            this._memoryRead.Location = new System.Drawing.Point(555, 2);
            this._memoryRead.Size = new System.Drawing.Size(40, 20);
            this._memoryRead.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._memoryRead.Click += new EventHandler(this.MemoryReadClicked);


            //
            //_memoryWrite Button
            //
            this._memoryWrite.Text = "MW";
            this._memoryWrite.Location = new System.Drawing.Point(513, 2);
            this._memoryWrite.Size = new System.Drawing.Size(40, 20);
            this._memoryWrite.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._memoryWrite.Click += new EventHandler(this.MemoryWriteClicked);

            //
            //_memoryRemove Button
            //
            this._memoryRemove.Text = "MC";
            this._memoryRemove.Location = new System.Drawing.Point(471, 2);
            this._memoryRemove.Size = new System.Drawing.Size(40, 20);
            this._memoryRemove.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._memoryRemove.Click += new EventHandler(this.MemoryRemoveClicked);

            //
            //_editButton
            //
            //this._editButton.Text = "Edit";
            //this._editButton.Location = new System.Drawing.Point(5, 2);
            //this._editButton.Size = new System.Drawing.Size(50, 20);
            //this._editButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            //
            //_helpButton
            //
            this._helpButton.Text = "Help";
            this._helpButton.Location = new System.Drawing.Point(5, 2);
            this._helpButton.Size = new System.Drawing.Size(50, 20);
            this._helpButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this._helpButton.Click += new EventHandler(this.HelpClicked);

            //
            //_exitButton
            //
            this._exitButton.Text = "Exit";
            this._exitButton.Location = new System.Drawing.Point(57, 2);
            this._exitButton.Size = new System.Drawing.Size(50, 20);
            this._exitButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this._exitButton.Click += new EventHandler(this.ExitClicked);


            // 
            // _inputTextBox : In this the input is shown and output result is displayed
            // 
            this._inputTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._inputTextBox.Location = new System.Drawing.Point(5, 55);
            this._inputTextBox.Margin = new System.Windows.Forms.Padding(500);
            this._inputTextBox.Name = "_inputTextBox";
            this._inputTextBox.Size = new System.Drawing.Size(590, 30);
            this._inputTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            this._inputTextBox.TextAlign = HorizontalAlignment.Right;

            //
            //_outputExpression label : In this the preview of the input expression is shown above the _inputTextBox textbox
            //
            this._outputExpression.AutoSize = false;
            this._outputExpression.Location = new System.Drawing.Point(5, 29);
            this._outputExpression.Name = "_outputExpression";
            this._outputExpression.Size = new System.Drawing.Size(590, 20);
            this._outputExpression.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._outputExpression.Anchor = AnchorStyles.Right;
            this._outputExpression.TextAlign = ContentAlignment.MiddleRight;
            this._outputExpression.Text = string.Empty;


            //
            //_panelLayout : It is a vertical splitContainer in _calculatorLayout.Panel2 which divides button panel into operation panel and default keys panel
            //
            this._panelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelLayout.Location = new System.Drawing.Point(0, 0);
            this._panelLayout.Name = "_inputPanelLayout";
            this._panelLayout.IsSplitterFixed = true;
            this._panelLayout.Panel1.Controls.Add(this._inputPanelOperators);
            this._panelLayout.Panel2.Controls.Add(this._inputPanelDefaults);


            //
            //_inputPanelDefaults
            //
            this._inputPanelDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputPanelDefaults.Location = new System.Drawing.Point(0, 0);
            this._inputPanelDefaults.Name = "_inputPanelDefaults";
            this._inputPanelDefaults.ColumnCount = 3;
            this._inputPanelDefaults.RowCount = 5;

            for (int i = 0; i < this._inputPanelDefaults.ColumnCount; i++)
            {
                this._inputPanelDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / this._inputPanelDefaults.ColumnCount));
            }
            for (int i = 0; i <= this._inputPanelDefaults.RowCount; i++)
            {
                this._inputPanelDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / this._inputPanelDefaults.RowCount));
            }
            this._inputPanelDefaults.Size = new System.Drawing.Size(300, 570);
            this._panelLayout.SplitterDistance = 70;

            //
            //Filling buttons in _inputPanelDefaults
            //
            FillDefaultButtons();

            // 
            // _inputPanelOperators
            // 
            this._inputPanelOperators.Dock = System.Windows.Forms.DockStyle.Fill;
            this._inputPanelOperators.Location = new System.Drawing.Point(0, 0);
            this._inputPanelOperators.Name = "_inputPanelOperators";
            this._inputPanelOperators.Size = new System.Drawing.Size(300, 570);
            this._inputPanelOperators.ColumnCount = 3;//Fixing the column size
            for (int i = 0; i < this._inputPanelOperators.ColumnCount; i++)
            {
                this._inputPanelOperators.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / this._inputPanelOperators.ColumnCount));
            }
            this._inputPanelOperators.RowCount = (_symbolMap.Count() + 1) / this._inputPanelOperators.ColumnCount;
            float ratio = 100 / this._inputPanelOperators.RowCount;
            for (int i = 0; i <= this._inputPanelOperators.RowCount; i++)
            {
                this._inputPanelOperators.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, ratio));
            }

            //
            //buttons
            //
            FillBucket();


            for (int i = 0; i < _buttonList.Count(); i++)
            {
                _buttonList[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            // 
            // Calculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Size = new System.Drawing.Size(600, 500);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this._calculatorLayout);
            this.Name = "Calculator";
            this.Text = "CALCULATOR";
            this._panelLayout.Panel1.ResumeLayout(false);
            this._panelLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._panelLayout)).EndInit();
            this._panelLayout.ResumeLayout(false);
            this._calculatorLayout.Panel1.ResumeLayout(false);
            this._calculatorLayout.Panel1.PerformLayout();
            this._calculatorLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._calculatorLayout)).EndInit();
            this._calculatorLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void Button_clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            _input += button.Text;
            _inputTextBox.Text = _input;

        }

        private void EqualToClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            answer = 0;
            if (_input != _inputTextBox.Text)
            {
                _input = _inputTextBox.Text;
            }
            try
            {
                if (_input.Length > 1)
                {
                    answer = eval.Evaluator(_input);
                }
                else
                {
                    _inputTextBox.Text = _input;
                    return;
                }

            }

            catch (OperationNotFoundException exception)
            {
                _inputTextBox.Text = exception.Message;
                _input = string.Empty;
                return;
            }
            catch (InsufficientOperandsException exception)
            {
                _inputTextBox.Text = exception.Message;
                _input = string.Empty;
                return;
            }
            catch (DivideByZeroException exception)
            {
                _inputTextBox.Text = exception.Message;
                _input = string.Empty;
                return;
            }
            catch (ArgumentException exception)
            {
                _inputTextBox.Text = exception.Message;
                _input = string.Empty;
                return;
            }

            _inputTextBox.Text = answer.ToString();
            _outputExpression.Text = _input + button.Text;
            _input = answer.ToString();
        }

        private void ClearAllClicked(object sender, EventArgs e)
        {
            _input = string.Empty;
            _outputExpression.Text = string.Empty;
            _inputTextBox.Text = string.Empty;
        }

        private void BackspaceClicked(object sender, EventArgs e)
        {
            if (_input.Length > 0)
            {
                _input = _input.Substring(0, _input.Length - 1);
                _outputExpression.Text = string.Empty;
                _inputTextBox.Text = _input;
            }

        }

        private void MemoryReadClicked(object sender, EventArgs e)
        {
            if (_storage.Count > 0)
            {
                if (_input == string.Empty)
                {
                    _input = _storage.Peek().ToString();
                }
                else
                {
                    _input += _storage.Peek().ToString();
                }
                _inputTextBox.Text = _input;
            }
            else
            {
                _inputTextBox.Text = "No value is stored in memory";
            }
        }

        private void MemoryWriteClicked(object sender, EventArgs e)
        {
            if (answer != 0)
            {
                _storage.Push(answer);
            }
        }

        private void MemoryRemoveClicked(object sender, EventArgs e)
        {
            if (_storage.Count != 0)
            {
                _storage.Pop();
            }
            else
            {
                _inputTextBox.Text = "No value is stored in memory";
            }
        }

        private void ExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpClicked(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Each symbol of this calculator corresponds to a mathematical function or operator. To know more click Yes, otherwise click No.", "Help", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://docs.oracle.com/cd/E19455-01/806-2901/6jc3a4lu1/index.html");
            }
            
        }

    }

}
