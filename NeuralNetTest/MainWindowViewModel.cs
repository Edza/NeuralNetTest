using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NeuralNetTest
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<InputNeuron> _inputValues;
        public List<InputNeuron> InputValues
        {
            get
            {
                return this._inputValues;
            }
            set
            {
                this._inputValues = value;

                PropertyChanged(null, new PropertyChangedEventArgs("InputValues"));
            }
        }

        private ICommand _goCommand;
        public ICommand GoCommand
        {
            get
            {
                return this._goCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            this._inputValues = new List<InputNeuron>() {
                new InputNeuron(1),
                new InputNeuron(0.6),
                new InputNeuron(0.2)           
            };

            this._goCommand = new GoCommand(GoCommand_Executed);
            
        }

        private void GoCommand_Executed()
        {
            Neuron n1 = new InputNeuron(this.InputValues[0].InputValue);
            Neuron n2 = new InputNeuron(this.InputValues[1].InputValue);
            Neuron n3 = new InputNeuron(this.InputValues[2].InputValue);
            Neuron m1 = new Neuron();
            Neuron m2 = new Neuron();
            Neuron o1 = new Neuron();
            NeuronConnection n1m1 = new NeuronConnection(n1);
            NeuronConnection n1m2 = new NeuronConnection(n1);
            NeuronConnection n2m1 = new NeuronConnection(n2);
            NeuronConnection n2m2 = new NeuronConnection(n2);
            NeuronConnection n3m1 = new NeuronConnection(n3);
            NeuronConnection n3m2 = new NeuronConnection(n3);
            NeuronConnection m1o1 = new NeuronConnection(m1);
            NeuronConnection m2o1 = new NeuronConnection(m2);
            n1m1.NeuronWeight = 0.55555;
            o1.neuronDendrons = new List<NeuronConnection>()
            {
                m1o1,
                m2o1
            };
            m1.neuronDendrons = new List<NeuronConnection>()
            {
                n1m1,
                n2m1,
                n3m1
            };
            m2.neuronDendrons = new List<NeuronConnection>()
            {
                n1m2,
                n2m2,
                n3m2
            };
            string o1Result = o1.ComputeNeuronCharge().ToString();

            
            MessageBox.Show(o1Result);


            
 
        }

    }

    public class GoCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return true;
        }

        // neizmantojam
        public event EventHandler CanExecuteChanged;

        Action _goCommandFunction;

        public GoCommand(Action goCommandFunction)
        {
            this._goCommandFunction = goCommandFunction;
        }

        public void Execute(object parameter)
        {
            this._goCommandFunction();
        }
    }

    public class InputNeuron:Neuron
    {
        public double InputValue
        {
            get;
            set;
        }

        public InputNeuron(double value)
        {
            this.InputValue = value;
        }

        public override double ComputeNeuronCharge()
        {
            return InputValue;
        }
    }
    public class Neuron
    {
        public List<NeuronConnection> neuronDendrons;
        
        public virtual double ComputeNeuronCharge()
        {
            double sum = 0;
            foreach (NeuronConnection neuronConnection in neuronDendrons)
            {
              sum+= neuronConnection.startNeuron.ComputeNeuronCharge() * neuronConnection.NeuronWeight;
            }
            // Par katru savienojumo no ienākošajiem neironiem
            // (Savienojms.OtraGalaNeirons.Izrēķināto vērtību * Savienojums.Svars)
            // atgreižu summu

            return sum;
        }
    }
    public class NeuronConnection
    {
        


        public NeuronConnection(Neuron startNeuron)
        {
            this.startNeuron = startNeuron;
        }

        private double _neuronWeight = 1;
        public double NeuronWeight
        {
            get { return _neuronWeight; }
            set { _neuronWeight = value; }
        }
        

      public  Neuron startNeuron
        {
            get;
            set;
        }
    }
}
