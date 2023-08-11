using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using System.Globalization;

namespace HRVEstimation
{
    public partial class MainPage : TabbedPage
    {
        HeartRateVariabilityCalculator calculator = new HeartRateVariabilityCalculator();
        public MainPage()
        {
            InitializeComponent();
            inputDataEntry.TextChanged += InputDataEntry_TextChanged;
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                myCollectionView.ItemsSource = await App.myDatabase.ReadPersons();
            }
            catch
            {
            }
        }

        private void InputDataEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = e.NewTextValue;

            string filteredText = Regex.Replace(enteredText, @"[^0-9\s]", "");

            inputDataEntry.Text = filteredText;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string inputData = inputDataEntry.Text;
            List<double> rrIntervals = new List<double> { };

            if (string.IsNullOrEmpty(inputData) || string.IsNullOrEmpty(fullNameEntry.Text) ||
                string.IsNullOrEmpty(dateOfBirthEntry.Text) || string.IsNullOrEmpty(departmentEntry.Text) ||
                string.IsNullOrEmpty(medicalCardNumberEntry.Text))
            {
                await DisplayAlert("Попередження", "Будь ласка, заповніть усі поля", "OK");
            }
            else
            {
                rrIntervals = GetRRIntervalsFromConsole(inputData);

                double meanRR = calculator.CalculateMeanRR(rrIntervals);
                double sdnn = calculator.CalculateSDNN(rrIntervals);
                double sdsd = calculator.CalculateSDSD(rrIntervals);
                double rmssd = calculator.CalculateRMSSD(rrIntervals);
                double pnn50 = calculator.CalculatePNN50(rrIntervals);
                double pnn20 = calculator.CalculatePNN20(rrIntervals);

                meanRRLabel.Text = meanRR.ToString();
                sdnnLabel.Text = sdnn.ToString();
                sdsdLabel.Text = sdsd.ToString();
                rmssdLabel.Text = rmssd.ToString();
                pnn50Label.Text = pnn50.ToString();
                pnn20Label.Text = pnn20.ToString();

                AddPersonAndMedicalExamResultExample(fullNameEntry.Text, medicalCardNumberEntry.Text, dateOfBirthEntry.Text, departmentEntry.Text, meanRR, sdnn, sdsd, rmssd, pnn50, pnn20);
            }
        }

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                PersonModel selectedPatient = e.CurrentSelection[0] as PersonModel;

                if (selectedPatient != null)
                {
                    StringBuilder messageBuilder = new StringBuilder();
                    messageBuilder.AppendLine($"П. І. Б.: {selectedPatient.FullName}");
                    messageBuilder.AppendLine($"Дата Народження: {selectedPatient.DateOfBirth}");
                    messageBuilder.AppendLine($"Відділення: {selectedPatient.Department}");
                    messageBuilder.AppendLine();
                    messageBuilder.AppendLine("Результати обстеження:");

                    MedicalExamResultModel examResult = await App.myDatabase.GetMedicalExamResultByPersonId(selectedPatient.Id);

                    if (examResult != null)
                    {
                        messageBuilder.AppendLine($"Mean RR Interval: {examResult.MeanRR}");
                        messageBuilder.AppendLine($"SDNN: {examResult.SDNN}");
                        messageBuilder.AppendLine($"SDSD: {examResult.SDSD}");
                        messageBuilder.AppendLine($"RMSSD: {examResult.RMSSD}");
                        messageBuilder.AppendLine($"PNN50: {examResult.PNN50}");
                        messageBuilder.AppendLine($"PNN20: {examResult.PNN20}");
                    }
                    else
                    {
                        messageBuilder.AppendLine("Результати обстеження відсутні.");
                    }

                    await DisplayAlert("Інформація про пацієнта", messageBuilder.ToString(), "OK");
                }

                myCollectionView.SelectedItem = null;
            }
        }

        static List<double> GetRRIntervalsFromConsole(string inputData)
        {
            string[] intervalStrings = inputData.Split(' ');

            List<double> rrIntervals = new List<double>();
            foreach (string intervalString in intervalStrings)
            {
                if (double.TryParse(intervalString, out double interval))
                {
                    rrIntervals.Add(interval);
                }
            }

            return rrIntervals;
        }

        async Task UpdateFirstTabContent()
        {
            List<PersonModel> persons = await App.myDatabase.ReadPersons();
            myCollectionView.ItemsSource = persons;
        }

        public async void AddPersonAndMedicalExamResultExample(string fullName, string medicalCardNumber, string dateOfBirth, string department, double meanRR, double sdnn, double sdsd, double rmssd, double pnn50, double pnn20)
        {
            PersonModel person = new PersonModel
            {
                FullName = fullName,
                MedicalCardNumber = medicalCardNumber,
                DateOfBirth = DateTime.ParseExact(dateOfBirth, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Department = department,
                DateOfAppointment = DateTime.Now
            };

            await App.myDatabase.CreatePerson(person);

            MedicalExamResultModel medicalExamResult = new MedicalExamResultModel
            {
                PersonId = person.Id,
                MeanRR = meanRR,
                SDNN = sdnn,
                SDSD = sdsd,
                RMSSD = rmssd,
                PNN50 = pnn50,
                PNN20 = pnn20
            };

            await App.myDatabase.CreateResult(medicalExamResult);

            UpdateFirstTabContent();

            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Запис успішно виконаний.");

            await DisplayAlert("Успіх", messageBuilder.ToString(), "OK");
        }



    }
}
