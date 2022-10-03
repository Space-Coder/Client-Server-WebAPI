using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TexodeAPI.Services;

namespace TexodeAPI.Services
{
    public class MobileService : IMobileService
    {
        public List<MobileCard> Cards { get; set; }
        public MobileService()
        {
            if (Cards == null)
            {
                Cards = new List<MobileCard>();
            }
            if ((Cards == null || Cards.Count == 0) && File.Exists("cards.json"))
            {
               Task.WaitAll(LoadData());
            }
        }

        public List<MobileCard> GetAllData()
        {
            return Cards;
        }

        public Task<MobileCard> GetById(Guid id)
        {
            var result = Cards.FirstOrDefault(i => i.ID == id);
            return Task.FromResult(result);
        }

        public Task<MobileCard> GetByName(string name)
        {
            var result = Cards.FirstOrDefault(i => i.Name == name);
            return Task.FromResult(result);
        }

        public async Task AddCard(MobileCard mobile)
        {
            Cards.Add(mobile);
            await SaveData();
        }

        public async Task UpdateCard(MobileCard mobile)
        {
            var item = Cards.FirstOrDefault(i => i.ID == mobile.ID);
            if (item != null)
            {
                item.Image = mobile.Image;
                item.Name = mobile.Name;
                await SaveData();
            }
            else
            {
                throw new NullReferenceException("Попытка изменить не существующую карточку");
            }
        }

        public async Task RemoveCards(List<MobileCard> mobiles)
        {
            foreach (var item in mobiles)
            {
                var card = await GetById(item.ID);
                if (card != null)
                {
                    Cards.Remove(card);
                }
                else
                {
                    Console.WriteLine("Ошибка, карточки с таким ID не существует");
                }
            }
            await SaveData();
        }

        private async Task LoadData()
        {
            using (FileStream fs = new FileStream("cards.json", FileMode.OpenOrCreate))
            {
                Cards = await JsonSerializer.DeserializeAsync<List<MobileCard>>(fs);
            }
        }
        private async Task SaveData()
        {
            using (FileStream fs = new FileStream("cards.json", FileMode.Create))
            {
                await JsonSerializer.SerializeAsync<List<MobileCard>>(fs, Cards);
            }
        }

        
    }
}
