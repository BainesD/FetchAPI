namespace FetchAPI.Models
{
    public class Balance
    {
        //Balance is used to prevent returning JSON that has a timestamp and an Id of 0
        public string Payer { get; set; }
        public int Points { get; set; }
    }
}
