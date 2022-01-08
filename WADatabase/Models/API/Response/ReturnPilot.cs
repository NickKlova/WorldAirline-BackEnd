namespace WADatabase.Models.API.Response
{
    public class ReturnPilot
    {
        public int Id { get; set; }
        public int FlyingHours { get; set; }
        public ReturnAccount Account { get; set; }
    }
}