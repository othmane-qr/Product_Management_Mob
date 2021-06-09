using ProductManagement.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Produit>().Wait();
        }

        //Insert and Update new record
        public Task<int> SaveItemAsync(Produit produit)
        {
            if (produit.Id != 0)
            {
                return db.UpdateAsync(produit);
            }
            else
            {
                return db.InsertAsync(produit);
            }
        }  

        //Delete
        public Task<int> DeleteItemAsync(Produit produit)
        {
            return db.DeleteAsync(produit);
        }

        //Read All Items
        public Task<List<Produit>> GetItemsAsync()
        {
            return db.Table<Produit>().ToListAsync();
        }


        //Read Item
        public Task<Produit> GetItemAsync(int produitId)
        {
            return db.Table<Produit>().Where(i => i.Id == produitId).FirstOrDefaultAsync();
        }
    }

}
