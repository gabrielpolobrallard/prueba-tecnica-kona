
using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PruebaTecnica.DAL
{
    public static class DataAccess
    {
        public static string DBPath { get; set; }
        //public int CurrentCustomerId { get; set; }
        const string DBNAME = "pruebatecnica.sqlite";

        public static void CreateIfNotExists()
        {
            DBPath = Path.Combine(
               Windows.Storage.ApplicationData.Current.LocalFolder.Path, DBNAME);


            using (var db = new SQLite.SQLiteConnection(DBPath))
            {
                db.CreateTable<RootDTO>();
            }
        }

        public static List<RootDTO> GetRootCollection()
        {
            var result = new List<RootDTO>();
            using (var db = new SQLite.SQLiteConnection(DBPath))
            {
                result = db.Table<RootDTO>().ToList();
            }
            return result;
        }


        public static string SaveRootDTO(RootDTO rootDTO)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingCustomer = (db.Table<RootDTO>().Where(
                        c => c.Name == rootDTO.Name)).SingleOrDefault();

                    if (existingCustomer != null)
                    {
                        existingCustomer.Name = rootDTO.Name;
                        existingCustomer.Direccion = rootDTO.Direccion;
                        existingCustomer.Imagen = rootDTO.Imagen;
                        int success = db.Update(existingCustomer);
                    }
                    else
                    {
                        int success = db.Insert(new RootDTO()
                        {
                            Name = rootDTO.Name,
                            Direccion = rootDTO.Direccion,
                            Imagen = rootDTO.Imagen
                        });
                    }
                    result = "Guardado con Exito";
                }
                catch (Exception ex)
                {
                    result = $"Fallo el guardado. Exception: {ex} ";
                }
            }
            return result;
        }



    }
}
