namespace RestKaniner.Models
{
    public class Kanin
    {
        public int Id { get; set; }
        public string? Navn { get; set; }
        public string? Farve { get; set; }
        public double Vægt { get; set; }
        public int? MorsId { get; set; }

        public override string ToString()
        {
            return $"Kanin(Id={Id}, Navn={Navn}, Farve={Farve}, Vægt={Vægt}, MorsId={MorsId})";
        }

    }
}
