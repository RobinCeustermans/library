using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Bibliotheek_DAL.UnitsOfWork;
using Bibliotheek_DAL;

namespace The_Boys_Project.Views
{
    /// <summary>
    /// Interaction logic for MailEditorView.xaml
    /// </summary>
    public partial class MailEditorView : UserControl
    {
        public MailEditorView()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var text = JsonConvert.DeserializeObject(await _webView.ExecuteScriptAsync("getText()")).ToString();
            
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).MailText = text;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string selectedMail = "";
            if (this.DataContext != null)
            {
                selectedMail = ((dynamic)this.DataContext).SelectedMailToEdit;
            }

            IUnitOfWork unitOfWork = new UnitOfWork(new LibraryEntities());

            string mailText = unitOfWork.EmailTextRepo.GetEntities(x => x.Description == selectedMail).FirstOrDefault().HTMLString;
            if (!string.IsNullOrWhiteSpace(mailText))
            {
                mailText = mailText.Replace('\'', '"');
                await _webView.ExecuteScriptAsync("setText(\'" + mailText + "\')");
            }
        }
    }
}
