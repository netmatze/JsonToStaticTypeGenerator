using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParserCombinators;
using ParserCombinators.JsonObjects;
using JsonConverter;

namespace JsonToStaticTypeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtClose_Click(object sender, RoutedEventArgs e)
        {
            //HelloClass hello = new HelloClass();
            //Proxy<HelloClass> proxy = new Proxy<HelloClass>(hello);
            //HelloClass hello2 = proxy.GetTransparentProxy() as HelloClass;
            //var result = hello2.Value;
            //Debug.Write(result);
            this.Close();
        }

        private void txtGenerate_Click(object sender, RoutedEventArgs e)
        {
            JsonParser jsonParser = new JsonParser();
            JsonObject jsonObject = jsonParser.Deserialize(txtParserText.Text);
            JsonSource.JsonObject_ = jsonObject;
            CodeGenerator codeGenerator = new CodeGenerator();
            var code = codeGenerator.Generate(jsonObject, txtNamespace.Text, txtClassname.Text);
            if (chkCopyToClipboard.IsChecked.Value)
            {
                Clipboard.SetDataObject(code);
            }
            CodeFileGenerator codeFileGenerator = new CodeFileGenerator();
            codeFileGenerator.Generate(txtFolder.Text, txtClassname.Text, code);
            MessageBox.Show(string.Format("Code file {0}\\{1}.cs generated.", 
                txtFolder.Text, txtClassname.Text));
        }

        private void btnSelectSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            var jsonConverter = new JsonConverter<MainTest.Addresse>();
            MainTest.Addresse address = jsonConverter.Deserialize(
                "{ 'street' : 'Gürtel', 'number' : 23, 'city' : 'Vienna' }");
            var street = address.street;
            var number = address.number;
            var city = address.city;
            //Trace.WriteLine(street);
            //Trace.WriteLine(number);
            //Trace.WriteLine(city);
            //var prop3 = address.item1_.prop3;
            //var prop4 = address.item2_.prop4;            
        }                
    }

    //public class Address : JsonStaticObject
    //{
    //    public string prop1 { get { return JsonSource.Find<string>("Addresse", "prop1"); } }
    //    public double prop2 { get { return JsonSource.Find<double>("Addresse", "prop2"); } }
    //    public double[] array1 { get { return JsonSource.FindArrayNumber("Addresse", "array1"); } }
    //    public item1 item1_ { get { return JsonSource.FindObject<item1>("item1", "item1"); } }
    //    public class item1 : JsonStaticObject
    //    {
    //    public string prop3 { get { return JsonSource.Find<string>("item1", "prop3"); } }
    //    }
    //    public item2 item2_ { get { return JsonSource.FindObject<item2>("item2", "item2"); } }
    //    public class item2 : JsonStaticObject
    //    {
    //        public string prop4 { get { return JsonSource.Find<string>("item2", "prop4"); } }
    //    }
    //}

    //public class HelloClass : MarshalByRefObject
    //{
    //    public string Value
    //    {
    //        get { return String.Empty; }
    //    }

    //    public string SayHello(string s)
    //    {
    //        s = "Hello World!";
    //        return s;
    //    }
    //}
}
