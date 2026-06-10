using System.Collections.Generic;
using System.Linq;

namespace RestKaniner.Models
{
    public class KaninRepository
    {
        private readonly List<Kanin> kaniner = new List<Kanin>();
        private int nextId = 1;

        public KaninRepository(bool includeData = false)
        {
            if (includeData)
            {
                Add(new Kanin { Navn = "Ninus", Farve = "Hvid", Vægt = 5, MorsId = null });
                Add(new Kanin { Navn = "Karl", Farve = "RødBrun", Vægt = 4.5, MorsId = 1 });
                Add(new Kanin { Navn = "Misse", Farve = "Hvid", Vægt = 4.6, MorsId = 1 });
                Add(new Kanin { Navn = "Plet", Farve = "Brun", Vægt = 5.6, MorsId = 2 });
            }
        }

        public IEnumerable<Kanin> GetAll()
        {
            return kaniner;
        }

        public Kanin? GetById(int id)
        {
            return kaniner.FirstOrDefault(k => k.Id == id);
        }

        public Kanin Add(Kanin kanin)
        {
            if (kanin is null)
            {
                throw new ArgumentNullException(nameof(kanin));
            }

            kanin.Id = nextId++;
            kaniner.Add(kanin);

            return kanin;
        }

        public Kanin? Delete(int id)
        {
            var kanin = GetById(id);

            if (kanin != null)
            {
                kaniner.Remove(kanin);
                return kanin;
            }

            return null;
        }

        //filter og sortering
        public IEnumerable<Kanin> FilterAndSort(string? farve, string? sort)
        {
            var result = kaniner.AsEnumerable();

            // Filter
            if (!string.IsNullOrEmpty(farve))
            {
                result = result.Where(k => k.Farve == farve);
            }

            // Sort
            if (sort == "asc")
            {
                result = result.OrderBy(k => k.Vægt);
            }
            else if (sort == "desc")
            {
                result = result.OrderByDescending(k => k.Vægt);
            }

            return result;
        }


    }
}