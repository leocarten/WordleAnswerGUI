using System.Net.Http;
using System.Text.Json;

namespace WordleAnswerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //    string[] array = dateTimePicker.Text.Split(',');
            //    string str = "";
            //    foreach(string word in array)
            //    {
            //        str += word;
            //    }
            //    MessageBox.Show(str);
        }

        private string mapMonthToNum(string month) 
        {
            if (month.Contains("January"))
            {
                return "01";
            }
            else if (month.Contains("February"))
            {
                return "02";
            }
            else if (month.Contains("March"))
            {
                return "03";
            }
            else if (month.Contains("April"))
            {
                return "04";
            }
            else if (month.Contains("May"))
            {
                return "05";
            }
            else if (month.Contains("June"))
            {
                return "06";
            }
            else if (month.Contains("July"))
            {
                return "07";
            }
            else if (month.Contains("August"))
            {
                return "08";
            }
            else if (month.Contains("September"))
            {
                return "09";
            }
            else if (month.Contains("October"))
            {
                return "10";
            }
            else if (month.Contains("November"))
            {
                return "11";
            }
            else if (month.Contains("December"))
            {
                return "12";
            }
            else
            {
                return "0";
            }
        }

        private async Task<string> MakeRequest(string year, string month, string day) 
        {            
            string url = $"https://www.nytimes.com/svc/wordle/v2/{year}-{month}-{day}.json";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //await Task.Delay(5000); // simulate network latency lol

                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse)) // parse the json
                    {
                        if (doc.RootElement.TryGetProperty("solution", out JsonElement solutionElement))
                        {
                            return solutionElement.GetString();
                        }
                        else
                        {
                            return null;
                        }
                    }

                    return jsonResponse;
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("There was an error trying to get the data you requested.");
                    return null;
                }
            }
        }


        private async void button1_Click_1(object sender, EventArgs e)
        {
            string dateChosen = dateTimePicker1.Text;
            string[] arr = dateChosen.Split(',');

            // format month
            string month = arr[1];
            string monthToInt = mapMonthToNum(month);

            // format year
            string year = arr[2];
            year = year.Trim(); //remove any additional white space

            // format day
            int length_of_second_item = arr[1].Length;
            char first_char_of_day = arr[1][length_of_second_item - 2];
            char second_char_of_day = arr[1][length_of_second_item - 1];
            if(first_char_of_day == ' ' || first_char_of_day == null)
            {
                first_char_of_day = '0';
            }
            string day = first_char_of_day.ToString();
            day += second_char_of_day;

            // now we need to make the request using a TASK
            string ans = await MakeRequest(year, monthToInt, day); // do this without blocking the UI thread!! window is still responsive
            if (ans != null) // make sure the request was successful
            {
                MessageBox.Show($"The wordle of the day on {dateChosen} is: {ans}");
            }
        }
    }
}
